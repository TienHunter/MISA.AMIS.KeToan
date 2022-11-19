using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.Common.Enums;
using MISA.AMIS.KeToan.Common.Exceptions;
using MISA.AMIS.KeToan.Common.Resources;
using MISA.AMIS.KeToan.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
        public dynamic InsertEmployee(Employee employee)
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
                errorMoreDup.Add("EmployeeCode", ResourceVN.ValidateError_DuplicateEmployeeCode);
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
        public dynamic UpdateEmployee(Guid employeeID, Employee employee)
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
                errorMoreDup.Add("EmployeeCode", ResourceVN.ValidateError_DuplicateEmployeeCode);
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
        public dynamic DeleteEmployeeByID(Guid employeeID)
        {
            return _employeeDL.DeleteEmployeeByID(employeeID);
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
            }

            //1.3 Thông tin tên nhân viên không để trống
            if (string.IsNullOrEmpty(employee.EmployeeName))
            {
                errorMore.Add("EmployeeName", ResourceVN.ValidateError_EmployeeNameNotEmpty);
            }

            //1.4 Thông tin phòng ban không để trống

            //1.5 Nếu có email thì email phỉa đúng định dạng
            if (!string.IsNullOrEmpty(employee.Email) && !IsValidEmail(employee.Email))
            {
                errorMore.Add("Email", ResourceVN.ValidateError_Email);
            }

            //1.6 Ngày sinh không lớn hơn ngày hiện tại
            if(!employee.DateOfBirth.HasValue && employee.DateOfBirth > DateTime.Now)
            {
                errorMore.Add("DateOfBirth", ResourceVN.ValidateError_DateOfBirth);
            }
            return errorMore;
        }

    }
}
