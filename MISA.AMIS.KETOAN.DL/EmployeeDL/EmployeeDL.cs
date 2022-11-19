using Dapper;
using MISA.AMIS.KeToan.Common;
using MISA.AMIS.KeToan.Common.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.DL
{


    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {

        #region Field

        // Khởi tạo kết nối tới DB MySQL
        private string connectionString = DatabaseContext.ConnectionString;

        #endregion

        #region Methods


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
        public PagingResult GetEmployeesByFilterAndPaging(string keyword, int limit, int offset, string sort)
        {



            //Chuẩn bị câu lệnh SQL
            string storedProcedureName = Procedure.GET_EMPLOYEES_BY_FILTER_PAGING;

            //Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add("@Limit", limit);
            parameters.Add("@Offset", offset);
            parameters.Add("@Sort", sort);
            parameters.Add("@KeySearch", keyword);

            //Khởi tạo kết nối tới DB MySQL
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                //Thực hiện gọi vào DB
                var results = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                var employees = results.Read<Employee>().ToList();
                long TotalRecords = results.Read<long>().Single();
                long TotalPages = (long)Math.Ceiling((double)TotalRecords / limit);

                //Xử lý kết quả trả về
                if (employees != null)
                {
                    return new PagingResult
                    {
                        Data = employees,
                        TotalRecords = TotalRecords,
                        TotalPages = TotalPages
                    };
                }
                // không có bản ghi nào trong db
                return new PagingResult
                {
                    Data = new List<Employee>(),
                    TotalRecords = 0,
                    TotalPages = 0
                };

            }



        }

        /// <summary>
        ///     API thêm mới 1 nhân viên
        /// </summary>
        /// <param name="employee">Đối tượng nhân viên cần thêm mới</param>
        /// <returns>ID nhân viên vừa thêm mới, số bản ghi bị ảnh hưởng</returns>
        /// Created by: VDTien (01/11/2022)
        public dynamic InsertEmployee(Employee employee)
        {

            //Chuẩn bị câu lệnh SQL
            string storedProcedureName = Procedure.INSERT_EMPLOYEE;

            //Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            var newEmployeeID = Guid.NewGuid();
            employee.EmployeeID = newEmployeeID;

            // map property của employee với tham số truyền vào database
            foreach (var prop in employee.GetType().GetProperties())
            {

                parameters.Add("@" + prop.Name, prop.GetValue(employee, null));
            }

            // Khởi tạo kết nối tới DB MySQL
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                //Thực hiện gọi vào DB
                int numberOfRowsAffected = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về
                dynamic result = new ExpandoObject();
                result.EmployeeID = newEmployeeID;
                result.numberOfRowsAffected = numberOfRowsAffected;
                return result;
            }

        }

        /// <summary>
        /// API sửa thông tin nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID nhân viên muốn sửa</param>
        /// <param name="employee">Đối tượng nhân viên muốn sửa</param>
        /// <returns>ID của nhân viên vừa sửa</returns>
        /// CreatedBy: VDTien (1/11/2022)
        public dynamic UpdateEmployee(Guid employeeID, Employee employee)
        {

            //Chuẩn bị câu lệnh SQL
            string storedProcedureName = Procedure.UPDATE_EMPLOYEE_BY_ID;

            //Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            employee.EmployeeID = employeeID;

            // map property của employee với tham số truyền vào database
            foreach (var prop in employee.GetType().GetProperties())
            {

                parameters.Add("@" + prop.Name, prop.GetValue(employee, null));
            }

            // Khởi tạo kết nối tới DB MySQL
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                //Thực hiện gọi vào DB
                int numberOfRowsAffected = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về
                dynamic result = new ExpandoObject();
                result.EmployeeID = employeeID;
                result.numberOfRowsAffected = numberOfRowsAffected;
                return result;
            }
        }

        /// <summary>
        /// API xóa thông tin nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID nhân viên muốn xóa</param>
        /// <returns>ID nhân viên vừa xóa</returns>
        /// CreatedBy: VDTien (1/11/2022)
        public dynamic DeleteEmployeeByID(Guid employeeID)
        {

            //Chuẩn bị câu lệnh SQL
            string storedProcedureName = Procedure.DELETE_EMPLOYEE_BY_ID;

            //Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add("@EmployeeID", employeeID);

            // Khởi tạo kết nối tới DB MySQL
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                //Thực hiện gọi vào DB
                int numberOfRowsAffected = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về
                dynamic result = new ExpandoObject();
                result.EmployeeID = employeeID;
                result.numberOfRowsAffected = numberOfRowsAffected;
                return result;
            }

        }

        /// <summary>
        /// API xóa nhiều nhân viên theo danh sách ID
        /// </summary>
        /// <param name="listEmployeeID">danh sách id nhân viên muốn xóa</param>
        /// <returns>StatusCode200</returns>
        /// CreatedByL VDTIEN(1/11/2022)
        public int DeleteMultipleEmployeesByID(string listEmployeeID)
        {
            //Chuẩn bị câu lệnh SQL
            string storedProcedureName = Procedure.DELETE_BATCH_EMPLOYEE;

            //Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add("@ListID", listEmployeeID);

            // Khởi tạo kết nối tới DB MySQL
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open(); //mở kết nối
                using (var myTrans = mySqlConnection.BeginTransaction())
                {
                    //Thực hiện gọi vào DB
                    int numberOfRowsAffected = mySqlConnection.Execute(storedProcedureName, parameters, myTrans, commandType: System.Data.CommandType.StoredProcedure);
                    //Xử lý kết quả trả về
                    myTrans.Commit();
                    return 1;
                }
                return -1;
            }
        }

        /// <summary>
        /// API lấy mã nhân viên lớn nhất trong database
        /// </summary>
        /// <returns>mã nhân viên lớn nhất</returns>
        /// CreatedBy: VDTIEN (14/11/2022)
        public dynamic GetEmployeeCodeMax()
        {


            //Chuẩn bị câu lệnh SQL
            string storedProcedureName = Procedure.GET_EMPLOYEE_CODE_MAX;

            //Chuẩn bị tham số đầu vào


            // Khởi tạo kết nối tới DB MySQL
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                //Thực hiện gọi vào DB
                string employeeCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về
                return employeeCode;
            }
        }

        /// <summary>
        /// Kiểm tra mã nhân viên có trùng hay không
        /// </summary>
        /// <param name="EmployeeCode">Mã nhân viên</param>
        /// <param name="EmployeeID">ID nhên viên</param>
        /// <returns>true:đã bị trùng; false: không bị trùng</returns>
        /// CreatedBy:VDTIEN(18/11/2022)
        public bool checkDuplicateEmployeeCode(string EmployeeCode, Guid? EmployeeID)
        {
            //Chuẩn bị câu lệnh SQL
            string storedProcedureName = Procedure.CHECK_DUPLICATE_EMPLOYEE_CODE;

            //Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add("@EmployeeCode", EmployeeCode);
            parameters.Add("@EmployeeID", EmployeeID);

            // Khởi tạo kết nối tới DB MySQL
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                //Thực hiện gọi vào DB
                string employeeCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về
                if (string.IsNullOrEmpty(employeeCode))
                {
                    return false;
                }
                return true;
            }

        }

        #endregion

    }
}
