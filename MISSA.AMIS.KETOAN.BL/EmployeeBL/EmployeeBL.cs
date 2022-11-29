using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.Common.Entities.DTO;
using MISA.AMIS.KeToan.Common.Enums;
using MISA.AMIS.KeToan.Common.Exceptions;
using MISA.AMIS.KeToan.Common.Resources;
using MISA.AMIS.KeToan.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;

namespace MISA.AMIS.KeToan.BL
{
    public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
    {
        #region Field

        private IEmployeeDL _employeeDL;

        #endregion

        #region Constructor

        public EmployeeBL(IEmployeeDL employeeDL) : base(employeeDL)
        {
            _employeeDL = employeeDL; 
        }

        #endregion

        /// <summary>
        ///     API thêm mới 1 nhân viên
        /// </summary>
        /// <param name="employee">Đối tượng nhân viên cần thêm mới</param>
        /// <returns>ID nhân viên vừa thêm mới</returns>
        /// Created by: VDTien (01/11/2022)
        public ActionResult InsertEmployee(Employee employee)
        {
            // thực hiện validate dữ liệu
            var errorMore = validateEmployee(employee);
            if(errorMore.Count > 0)
            {
                throw new ValidateException(errorMore);
            }

            // kiểm tra trùng mã 
            var isDuplicate = checkDuplicateEmployeeCode(employee.EmployeeCode, null);
            if(isDuplicate == true)
            {
                var errorMoreDup = new Dictionary<string, string>();
                errorMoreDup.Add("EmployeeCode", ResourceVN.UserMsg_DuplicateEmployeeCode);
                throw new DuplicateException(errorMoreDup);
            }

            // thực hiện thêm mới dữ liệu   
            return _employeeDL.InsertEmployee(employee);
        }

        /// <summary>
        /// API sửa thông tin nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID nhân viên muốn sửa</param>
        /// <param name="employee">Đối tượng nhân viên muốn sửa</param>
        /// <returns>ID của nhân viên vừa sửa</returns>
        /// CreatedBy: VDTien (1/11/2022)
        public ActionResult UpdateEmployee(Guid employeeID, Employee employee)
        {
            // thực hiện validate dữ liệu
            var errorMore = validateEmployee(employee);
            if (errorMore.Count > 0)
            {
                throw new ValidateException(errorMore);
            }

            // kiểm tra trùng mã 
            var isDuplicate = checkDuplicateEmployeeCode(employee.EmployeeCode, employeeID);
            if (isDuplicate == true)
            {
                var errorMoreDup = new Dictionary<string, string>();
                errorMoreDup.Add("EmployeeCode", ResourceVN.UserMsg_DuplicateEmployeeCode);
                throw new DuplicateException(errorMoreDup);
            }

            return _employeeDL.UpdateEmployee(employeeID, employee);
        }

        /// <summary>
        /// API lấy mã nhân viên lớn nhất trong database
        /// </summary>
        /// <returns>mã nhân viên lớn nhất</returns>
        /// CreatedBy: VDTIEN (14/11/2022)
        public string GetEmployeeCodeMax()
        {
            return _employeeDL.GetEmployeeCodeMax();
        }

        /// <summary>
        /// API xóa thông tin nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID nhân viên muốn xóa</param>
        /// <returns>ID nhân viên vừa xóa</returns>
        /// CreatedBy: VDTien (1/11/2022)
        public  ActionResult DeleteEmployeeByID(Guid employeeID)
        {
            return  _employeeDL.DeleteEmployeeByID(employeeID);
        }

        /// <summary>
        /// API xóa nhiều nhân viên theo danh sách ID
        /// </summary>
        /// <param name="listEmployeeID">danh sách id nhân viên muốn xóa</param>
        /// <returns>StatusCode200</returns>
        /// CreatedByL VDTIEN(1/11/2022)
        public int DeleteMultipleEmployeesByID(string listEmployeeID)
        {
            return _employeeDL.DeleteMultipleEmployeesByID(listEmployeeID);
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
            return _employeeDL.checkDuplicateEmployeeCode(EmployeeCode, EmployeeID);
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
        public PagingResult GetEmployeesByFilterAndPaging(string keyword, int limit, int offset, string sort)
        {
            return _employeeDL.GetEmployeesByFilterAndPaging(keyword, limit, offset, sort);
        }

        /// <summary>
        /// API lấy danh sách nhân viên theo bộ lọc
        /// </summary>
        /// <param name="keyword">Từ khóa muốn tìm kiếm</param>
        /// <returns>Danh sách nhân viên</returns>
        /// CreatedBy: VDTIEN(24/11/2022)
        public List<Employee> GetEmployeesByFilter(string keyword)
        {
            return _employeeDL.GetEmployeesByFilter(keyword);
        }

        /// <summary>
        /// Kiểm tra định dạng Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true: đúng định dang email; false: sai định dạng email</returns>
        /// CreatedBy:VDTIEN(18/11/2022)
        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// validate thông tin nhân viên truyền từ fe vào
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>danh sách các lỗi</returns>
        /// CreatedBy: VDTIEN (18/11/2022)
        private Dictionary<string, string> validateEmployee(Employee employee)
        {
            var errorMore = new Dictionary<string, string>();
            //1.1 Thông tin mã số nhân viên không để trống
            if (string.IsNullOrEmpty(employee.EmployeeCode))
            {
                errorMore.Add("EmployeeCode", ResourceVN.ValidateError_EmployeeCodeNotEmpty);
            } else
            {
                // 1.2 Kiểm tra định dạnh mã nhân viên
                string patten = @"^(NV-)(\d+)$";
                if(!Regex.IsMatch(employee.EmployeeCode, patten))
                {
                    errorMore.Add("EmployeeCode", ResourceVN.ValidateError_EmployeeCodeFormat);
                }
            }

            //1.3 Thông tin tên nhân viên không để trống
            if (string.IsNullOrEmpty(employee.EmployeeName))
            {
                errorMore.Add("EmployeeName", ResourceVN.ValidateError_EmployeeNameNotEmpty);
            }

            //1.4 Thông tin phòng ban không để trống
            if(string.IsNullOrEmpty(employee.DepartmentID.ToString()))
            {
                errorMore.Add("DepartmentID", ResourceVN.ValidateError_DepartmentIDNotEmpty);
            }
            //1.5 Nếu có email thì email phỉa đúng định dạng
            if (!string.IsNullOrEmpty(employee.Email) && !IsValidEmail(employee.Email))
            {
                errorMore.Add("Email", ResourceVN.ValidateError_Email);
            }

            //1.6 Ngày sinh không lớn hơn ngày hiện tại
            if(employee.DateOfBirth.HasValue && employee.DateOfBirth > DateTime.Now)
            {
                errorMore.Add("DateOfBirth", ResourceVN.ValidateError_DateOfBirth);
            }
            return errorMore;
        }

        /// <summary>
        /// xuất khẩu dữ liệu
        /// </summary>
        /// <param name="records">Danh sách nhân viên xuất khẩu</param>
        /// <returns>file excel</returns>
        /// CreatedBy: VDTIEN(24/11/2022)
        public MemoryStream ExportEmployees(List<Employee> records)
        {
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
                workSheet.Cells["A1:I1"].Style.Font.SetFromFont("Arial", 16, bold: true);
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
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(Color.Black);
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

                    workSheet.Cells[$"A{recorddRow}:$I{recorddRow}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[$"A{recorddRow}:$I{recorddRow}"].Style.Border.Bottom.Color.SetColor(Color.Black);
                    recorddRow++;
                }
                //make all text fit the cells
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                // căm giữa ngày sinh
                workSheet.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //i use this to make all columms just a bit wider, text would sometimes still overflow after AutoFitColumns(). Bug?
                for (int col = 1; col <= workSheet.Dimension.End.Column; col++)
                {
                    workSheet.Column(col).Width = workSheet.Column(col).Width + 1;
                    workSheet.Cells[3, col, recorddRow - 1, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[3, col, recorddRow - 1, col].Style.Border.Right.Color.SetColor(Color.Black);
                }

                package.Save();
            }
            stream.Position = 0;
            return stream;

        }
       

        private string convertGenderName(Gender? gender)
        {
            string genderName = "";
            switch (gender)
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

    }
}
