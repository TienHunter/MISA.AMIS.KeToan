using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.DL;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
