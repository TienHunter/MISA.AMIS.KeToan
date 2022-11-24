
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
using MISA.AMIS.KeToan.Common.Resources;
using MISA.AMIS.KeToan.DL;
using System.Reflection;
using System.Dynamic;
using MISA.AMIS.KeToan.Common.Exceptions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using OfficeOpenXml.Table;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.ComTypes;
using System;

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
                return StatusCode(StatusCodes.Status200OK, "0");

                //Thành công: trả về dữ liệu cho FE

                //Thất bại: trả về lỗi
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = ResourceVN.DevMsg_Exception,
                    UserMsg = ResourceVN.UserMsg_Exception,
                    MoreInfo = ResourceVN.MoreInfo_Exception,
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
                var result = _employeeBL.GetEmployeesByFilterAndPaging(keyword, limit, offset, sort);
                //Xử lý kết quả trả về

                return StatusCode(StatusCodes.Status200OK, result);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return HandleException(e);
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
                // thực hiện thêm mới dữ liệu
                var result = _employeeBL.InsertEmployee(employee);

                //Xử lý kết quả trả về
                //Thành công: trả về dữ liệu cho FE
                if (result.numberOfRowsAffected > 0)
                {
                    return StatusCode(StatusCodes.Status201Created, result.EmployeeID);
                }

                //Thất bại: trả về lỗi
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = AMISErrorCode.InValidData,
                    DevMsg = ResourceVN.DevMsg_ErrorInsert,
                    UserMsg = ResourceVN.UserMsg_ErrorInsert,
                    MoreInfo = ResourceVN.MoreInfo_ErrorInsert,
                    TraceId = HttpContext.TraceIdentifier
                });


            }
            catch (ValidateException e)
            {
                Console.WriteLine(e.errorMore);
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                {
                    ErrorCode = AMISErrorCode.InValidData,
                    DevMsg = ResourceVN.DevMsg_InValidData,
                    UserMsg = ResourceVN.ValidateError_Input,
                    MoreInfo = e.errorMore,
                    TraceId = HttpContext.TraceIdentifier
                });
            }
            catch (DuplicateException e)
            {
                Console.WriteLine(e.errorMore);
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                {
                    ErrorCode = AMISErrorCode.DuplicateCode,
                    DevMsg = ResourceVN.DevMsg_DuplicateEmployeeCode,
                    UserMsg = e.errorMore.First().Value,
                    MoreInfo = e.errorMore,
                    TraceId = HttpContext.TraceIdentifier
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return HandleException(e);
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

                // thực hiện update dữ liệu
                var result = _employeeBL.UpdateEmployee(employeeID, employee);
                //Xử lý kết quả trả về
                if (result.numberOfRowsAffected > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, result.EmployeeID);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = AMISErrorCode.InValidData,
                    DevMsg = ResourceVN.DevMsg_UpdateFailed,
                    UserMsg = ResourceVN.UserMsg_UpdateFailed,
                    MoreInfo = ResourceVN.MoreInfo_UpdateFailed,
                    TraceId = HttpContext.TraceIdentifier
                });

                //Thất bại: trả về lỗi
            }
            catch (ValidateException e)
            {
                Console.WriteLine(e.errorMore);
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                {
                    ErrorCode = AMISErrorCode.InValidData,
                    DevMsg = ResourceVN.DevMsg_ErrorInsert,
                    UserMsg = ResourceVN.ValidateError_Input,
                    MoreInfo = e.errorMore,
                    TraceId = HttpContext.TraceIdentifier
                });
            }
            catch (DuplicateException e)
            {
                Console.WriteLine(e.errorMore);
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                {
                    ErrorCode = AMISErrorCode.DuplicateCode,
                    DevMsg = ResourceVN.DevMsg_DuplicateEmployeeCode,
                    UserMsg = e.errorMore.First().Value,
                    MoreInfo = e.errorMore,
                    TraceId = HttpContext.TraceIdentifier
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return HandleException(e);
            }
        }
        /// <summary>
        /// API xóa thông tin nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID nhân viên muốn xóa</param>
        /// <returns>ID nhân viên vừa xóa</returns>
        /// CreatedBy: VDTien (1/11/2022)
        [HttpDelete("{employeeID}")]
        public async Task<IActionResult> DeleteEmployeeByID([FromRoute] Guid employeeID)
        {

            try
            {
                var result = await _employeeBL.DeleteEmployeeByID(employeeID);
                //Xử lý kết quả trả về
                //Thành công: trả về dữ liệu cho FE
                if (result.numberOfRowsAffected > 0)
                {
                    return StatusCode(StatusCodes.Status201Created, result.EmployeeID);
                }

                //Thất bại: trả về lỗi
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = AMISErrorCode.InValidData,
                    DevMsg = "Database delete record failed.",
                    UserMsg = "Xóa nhân nhân viên không thành công !",
                    MoreInfo = "https://openapi.misa.com.vn/errorcode/1",
                    TraceId = HttpContext.TraceIdentifier
                });

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return HandleException(e);
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

            try
            {

                //Chuẩn bị tham số đầu vào
                string listID = string.Join("\",\"", listEmployeeID);
                listID = "\"" + listID + "\"";

                int result = _employeeBL.DeleteMultipleEmployeesByID(listID);

                //Thành công: trả về dữ liệu cho FE
                if (result == 1)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }

                //Thất bại: trả về lỗi
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 2,
                    DevMsg = "Database delete batch record failed.",
                    UserMsg = "Xóa nhân nhân viên hàng loạt không thành công !",
                    MoreInfo = "https://openapi.misa.com.vn/errorcode/1",
                    TraceId = HttpContext.TraceIdentifier
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return HandleException(e);
            }
        }

        /// <summary>
        /// trả về lỗi 500 exception
        /// </summary>
        /// <param name="e"></param>
        /// <returns>trả về lỗi 500 exception</returns>
        /// CreatedBy: VDTIEN(18/11/2022)
        private IActionResult HandleException(Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
            {
                ErrorCode = AMISErrorCode.Exception,
                DevMsg = e.Message,
                UserMsg = ResourceVN.UserMsg_Exception,
                MoreInfo = ResourceVN.MoreInfo_Exception,
                TraceId = HttpContext.TraceIdentifier
            });
        }

        /// <summary>
        /// API export dữ liệu ra file excel
        /// </summary>
        /// <param name="listEmployeeID">danh sách id nhân viên muốn xóa</param>
        /// <returns></returns>
        /// CreatedByL VDTIEN(23/11/2022)
        [HttpGet("export")]

        public IActionResult ExportEmployees([FromQuery] string? keyword="")
        {
            try
            {
                List<Employee> records = _employeeBL.GetEmployeesByFilter(keyword);
                if (records == null) records = new List<Employee>();
                // If you are a commercial business and have
                // purchased commercial licenses use the static property
                // LicenseContext of the ExcelPackage class:
                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                // If you use EPPlus in a noncommercial context
                // according to the Polyform Noncommercial license:
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    // Tạo author cho file Excel
                    package.Workbook.Properties.Author = "VDTIEN";

                    var workSheet = package.Workbook.Worksheets.Add("DANH_SACH_NHAN_VIEN");
                    //var workSheet = package.Workbook.Worksheets[0];

                    // Tạo title
                    workSheet.Cells["A1"].Value = "DANH SÁCH NHÂN VIÊN";
                    workSheet.Cells["A1:I1"].Merge = true;
                    workSheet.Cells["A1:I1"].Style.Font.SetFromFont("Arial",16,bold:true);
                    workSheet.Cells["A1:I1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //Tạo header
                    workSheet.Cells[3, 1].Value = "STT";
                    workSheet.Cells[3, 2].Value = "Mã nhân viên";
                    workSheet.Cells[3, 3].Value = "Tên nhân viên";
                    workSheet.Cells[3, 4].Value = "Giới tính";
                    workSheet.Cells[3, 5].Value = "Ngày sinh";
                    workSheet.Cells[3, 6].Value = "Chức danh";
                    workSheet.Cells[3, 6].Value = "Chức danh";
                    workSheet.Cells[3, 7].Value = "Tên đơn vị";
                    workSheet.Cells[3, 8].Value = "Số tài khoản";
                    workSheet.Cells[3, 9].Value = "Tên ngân hàng";

                    // Lấy range vào tạo format cho range đó ở đây là từ A1 tới D1
                    using (var range = workSheet.Cells["A3:I3"])
                    {
                        // Set PatternType
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        // Set Màu cho Background
                        range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        // Canh giữa cho các text
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        // Set Font cho text  trong Range hiện tại
                        range.Style.Font.SetFromFont("Arial", 10, bold: true);
                        // Set Border
                        range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Top.Color.SetColor(Color.Black);
                    }
                    // Đỗ dữ liệu từ list vào 
                    int recorddRow = 4;
                    for (int i = 0; i < records.Count; i++)
                    {
                    
                        var item = records[i];
                        workSheet.Cells[recorddRow, 1].Value = i + 1;
                        workSheet.Cells[recorddRow, 2].Value = item.EmployeeCode;
                        workSheet.Cells[recorddRow, 3].Value = item.EmployeeName;
                        workSheet.Cells[recorddRow, 4].Value = convertGenderName(item.Gender);
                        workSheet.Cells[recorddRow, 5].Value = convertDateOfBirth(item.DateOfBirth);
                        workSheet.Cells[recorddRow, 6].Value = item.JobPositionName;
                        workSheet.Cells[recorddRow, 7].Value = item.DepartmentName;
                        workSheet.Cells[recorddRow, 8].Value = item.BankAccountNumber;
                        workSheet.Cells[recorddRow, 9].Value = item.BankName;

                        workSheet.Row(recorddRow).Style.Font.SetFromFont("Times New Roman", 11);
                        workSheet.Row(recorddRow).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        recorddRow++;
                    }
                    //make all text fit the cells
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();

                    //i use this to make all columms just a bit wider, text would sometimes still overflow after AutoFitColumns(). Bug?
                    for (int col = 1; col <= workSheet.Dimension.End.Column; col++)
                    {
                        workSheet.Column(col).Width = workSheet.Column(col).Width + 1;
                        workSheet.Cells[3,col, recorddRow-1,col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, col, recorddRow - 1, col].Style.Border.Right.Color.SetColor(Color.Black);
                    }
                    for (int row = 3; row <= workSheet.Dimension.End.Row; row++)
                    {
                        workSheet.Cells[$"A{row}:$I{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[$"A{row}:$I{row}"].Style.Border.Bottom.Color.SetColor(Color.Black);
                    }

                    package.Save();
                }
                stream.Position = 0;
                string excelName = "Danh_Sach_Nhan_Vien" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            //Try catch exception 
            catch (Exception e)
            {
                Console.WriteLine("CreateExcelFile", e.Message);
                return HandleException(e);
            }
        }

        private string convertGenderName(Gender? gender)
        {
            string genderName = "";
            switch(gender)
            {
                case Gender.Male:
                    genderName = "Nam";
                    break;
                case Gender.Female:
                    genderName = "Nữ";
                    break;
                case Gender.Other:
                    genderName = "Khác";
                    break;
                default:
                    genderName = "";
                    break;
            }
            return genderName;
        }

        private string convertDateOfBirth(DateTime? dob)
        {
            if (dob.HasValue)
            {
                DateTime Dob = (DateTime)dob;
                return Dob.ToString("dd/MM/yyyy");
            }
            else return "";
        }
        #endregion
    }
}
