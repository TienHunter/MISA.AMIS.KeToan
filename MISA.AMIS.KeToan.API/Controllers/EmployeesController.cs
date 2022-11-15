
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.KeToan.Common.Entities;
using MySqlConnector;
using System.Diagnostics;
using System.Net.Http;
using System.Data.SqlClient;
using System.Data.Common;
using MISA.AMIS.KeToan.BL;
using System.Net.WebSockets;
using MISA.AMIS.KeToan.Common;
using MISA.AMIS.KeToan.Common.Enums;

namespace MISA.AMIS.KeToan.API.Controllers
{
    [ApiController]
    public class EmployeesController : BaseController<Employee>
    {
        #region Field

        private IEmployeeBL _employeeBL;

        #endregion

        #region Constructor

        public EmployeesController(IEmployeeBL employeeBL) : base(employeeBL)
        {
            _employeeBL = employeeBL;
        }

        #endregion


        #region Method


        /// <summary>
        /// API lấy mã nhân viên lớn nhất trong database
        /// </summary>
        /// <returns>mã nhân viên lớn nhất</returns>
        /// CreatedBy: VDTIEN (14/11/2022)
        [HttpGet("EmployeeCodeMax")]
        public IActionResult GetEmployeeCodeMax()
        {
            try
            {
                var employeeCode = _employeeBL.GetEmployeeCodeMax();
                //Xử lý kết quả trả về
                if (employeeCode != null)
                {
                    return StatusCode(StatusCodes.Status200OK, employeeCode);
                }
                return StatusCode(StatusCodes.Status200OK ,"NV00000");

                //Thành công: trả về dữ liệu cho FE

                //Thất bại: trả về lỗi
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = Resources.DevMsg_Exception,
                    UserMsg = Resources.UserMsg_Exception,
                    MoreInfo = Resources.MoreInfo_Exception,
                    TraceId = HttpContext.TraceIdentifier
                });
            }

            //Try catch exception
        }

        /// <summary>
        /// API lấy danh sách nhân viên theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa muốn tìm kiếm</param>
        /// <param name="departmentID">ID của phòng ban muốn lọc</param>
        /// <param name="limit">Số bản ghi muốn lấy</param>
        /// <param name="offset">Vị trí của bản ghi bắt đầu lấy</param>
        /// <param name="sort">Sắp xếp theo chiều nào</param>
        /// <returns>Danh sách nhân viên và tổng số bản ghi</returns>
        /// CreatedBy: VDTIEN(1/11/2022)
        [HttpGet("filter")]
        public IActionResult GetEmployeeByFilterAndPaging(
            [FromQuery] string? keyword,
            [FromQuery] int limit = 10,
            [FromQuery] int offset = 0,
            [FromQuery] string? sort = ""

        )
        {
            try
            {
                //Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web09.ctm.vdtien;Uid=root;Pwd=tien.hust;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Chuẩn bị câu lệnh SQL
                string storedProcedureName = "Proc_employee_Pagination";

                //Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                parameters.Add("@Limit", limit);
                parameters.Add("@Offset", offset);
                parameters.Add("@Sort", sort);
                parameters.Add("@KeySearch", keyword);

                //Thực hiện gọi vào DB
                var results = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                var employees = results.Read<dynamic>().ToList();
                long TotalRecords = results.Read<long>().Single();
                long TotalPages = (long)Math.Ceiling((double)TotalRecords /limit);
                //Xử lý kết quả trả về
                //Thành công: trả về dữ liệu cho FE
                if (employees != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new 
                    {
                        Data = employees,
                        TotalRecords,
                        TotalPages
                    });
                }
                //Thất bại: trả về lỗi
                return StatusCode(StatusCodes.Status200OK, new
                {
                    Data = new List<Employee>(),
                    TotalRecords = 0,
                    TotalPages=0
                }); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Catched an exception.",
                    UserMsg = "Có lỗi xảy ra! Vui lòng liên hệ với MISA.",
                    MoreInfo = "https://openapi.misa.com.vn/errorcode/1",
                    TraceId = HttpContext.TraceIdentifier
                });
            }

            //Try catch exception
        }

        /// <summary>
        ///     API thêm mới 1 nhân viên
        /// </summary>
        /// <param name="employee">Đối tượng nhân viên cần thêm mới</param>
        /// <returns>ID nhân viên vừa thêm mới</returns>
        /// Created by: VDTien (01/11/2022)
        [HttpPost]
        public IActionResult InsertEmployee([FromBody] Employee employee)
        {

            try
            {
                int numberOfRowsAffected = _employeeBL.InsertEmployee(employee);
                //Xử lý kết quả trả về
                //Thành công: trả về dữ liệu cho FE
                if (numberOfRowsAffected > 0)
                {
                    return StatusCode(StatusCodes.Status201Created, numberOfRowsAffected);
                }

                //Thất bại: trả về lỗi
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 2,
                    DevMsg = "Database insert failed.",
                    UserMsg = "Thêm mới nhân viên không thành công !",
                    MoreInfo = "https://openapi.misa.com.vn/errorcode/1",
                    TraceId = HttpContext.TraceIdentifier
                });


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Catched an exception.",
                    UserMsg = "Có lỗi xảy ra! Vui lòng liên hệ với MISA.",
                    MoreInfo = "https://openapi.misa.com.vn/errorcode/1",
                    TraceId = HttpContext.TraceIdentifier
                });
            }

            //Try catch exception
        }

        /// <summary>
        /// API sửa thông tin nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID nhân viên muốn sửa</param>
        /// <param name="employee">Đối tượng nhân viên muốn sửa</param>
        /// <returns>ID của nhân viên vừa sửa</returns>
        /// CreatedBy: VDTien (1/11/2022)
        [HttpPut("{employeeID}")]
        public IActionResult UpdateEmployee(
            [FromRoute] Guid employeeID,
            [FromBody] Employee employee
        )
        {
            try
            {
                int numberOfRowsAffected = _employeeBL.UpdateEmployee(employeeID, employee);
                //Xử lý kết quả trả về
                if (numberOfRowsAffected > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, numberOfRowsAffected);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 2,
                    DevMsg = "Database update failed.",
                    UserMsg = "Cập nhật nhân viên không thành công !",
                    MoreInfo = "https://openapi.misa.com.vn/errorcode/1",
                    TraceId = HttpContext.TraceIdentifier
                });

                //Thất bại: trả về lỗi
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Catched an exception.",
                    UserMsg = "Có lỗi xảy ra! Vui lòng liên hệ với MISA.",
                    MoreInfo = "https://openapi.misa.com.vn/errorcode/1",
                    TraceId = HttpContext.TraceIdentifier
                });
            }
        }
        /// <summary>
        /// API xóa thông tin nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID nhân viên muốn xóa</param>
        /// <returns>ID nhân viên vừa xóa</returns>
        /// CreatedBy: VDTien (1/11/2022)
        [HttpDelete("{employeeID}")]
        public IActionResult DeleteEmpolyee([FromRoute] Guid employeeID)
        {

            try
            {
                //Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web09.ctm.vdtien;Uid=root;Pwd=tien.hust;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Chuẩn bị câu lệnh SQL
                string storedProcedureName = "Proc_employee_DeleteByID";

                //Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID);

                //Thực hiện gọi vào DB
                var numberOfRowsAffected = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về
                if (numberOfRowsAffected > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, employeeID);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 2,
                    DevMsg = "Database delete record failed.",
                    UserMsg = "Xóa nhân nhân viên không thành công !",
                    MoreInfo = "https://openapi.misa.com.vn/errorcode/1",
                    TraceId = HttpContext.TraceIdentifier
                });

                //Thành công: trả về dữ liệu cho FE

                //Thất bại: trả về lỗi
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Catched an exception.",
                    UserMsg = "Có lỗi xảy ra! Vui lòng liên hệ với MISA.",
                    MoreInfo = "https://openapi.misa.com.vn/errorcode/1",
                    TraceId = HttpContext.TraceIdentifier
                });
            }

            //Try catch exception
        }

        /// <summary>
        /// API xóa nhiều nhân viên theo danh sách ID
        /// </summary>
        /// <param name="listEmployeeID">danh sách id nhân viên muốn xóa</param>
        /// <returns>StatusCode200</returns>
        /// CreatedByL VDTIEN(1/11/2022)
        [HttpPost("deleteBatch")]
        public IActionResult DeleteMultipleEmployees([FromBody] List<Guid> listEmployeeID)
        {
            //Khởi tạo kết nối tới DB MySQL
            string connectionString = "Server=localhost;Port=3306;Database=misa.web09.ctm.vdtien;Uid=root;Pwd=tien.hust;";
            var mySqlConnection = new MySqlConnection(connectionString);
            mySqlConnection.Open(); //mở kết nối
            MySqlTransaction myTrans;

            // Start a local transaction
            myTrans = mySqlConnection.BeginTransaction();
            //Chuẩn bị câu lệnh SQL
            string storedProcedureName = "Proc_employee_deleteMultiEmployee";
            try
            {


                //Chuẩn bị tham số đầu vào
                string listID = string.Join("\",\"", listEmployeeID);
                listID = "\"" + listID + "\"";
                var parameters = new DynamicParameters();
                parameters.Add("@ListID", listID);
                Console.WriteLine(listID);
                //Thực hiện gọi vào DB
                var numberOfRowsAffected = mySqlConnection.Execute(storedProcedureName, parameters, myTrans, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về
                myTrans.Commit();
                return StatusCode(StatusCodes.Status200OK, 1);
                /*               if (numberOfRowsAffected > listEmployeeID)
                                {
                                    return StatusCode(StatusCodes.Status200OK, employeeID);
                                }
                                return StatusCode(StatusCodes.Status500InternalServerError, new
                                {
                                    ErrorCode = 2,
                                    DevMsg = "Database delete record failed.",
                                    UserMsg = "Xóa nhân nhân viên không thành công !",
                                    MoreInfo = "https://openapi.misa.com.vn/errorcode/1",
                                    TraceId = HttpContext.TraceIdentifier
                                });*/

                //Thành công: trả về dữ liệu cho FE

                //Thất bại: trả về lỗi
            }
            catch (Exception e)
            {
                myTrans.Rollback();
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Catched an exception.",
                    UserMsg = "Có lỗi xảy ra! Vui lòng liên hệ với MISA.",
                    MoreInfo = "https://openapi.misa.com.vn/errorcode/1",
                    TraceId = HttpContext.TraceIdentifier
                });
            }
            finally
            {
                mySqlConnection.Close();
            }

        }
        #endregion
    }
}
