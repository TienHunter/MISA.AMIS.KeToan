using MISA.AMIS.KeToan.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.BL
{
    public interface IEmployeeBL : IBaseBL<Employee>
    {

        /// <summary>
        ///     API thêm mới 1 nhân viên
        /// </summary>
        /// <param name="employee">Đối tượng nhân viên cần thêm mới</param>
        /// <returns>ID nhân viên vừa thêm mới</returns>
        /// Created by: VDTien (01/11/2022)
        public int InsertEmployee(Employee employee);

        /// <summary>
        /// API sửa thông tin nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID nhân viên muốn sửa</param>
        /// <param name="employee">Đối tượng nhân viên muốn sửa</param>
        /// <returns>ID của nhân viên vừa sửa</returns>
        /// CreatedBy: VDTien (1/11/2022)
        public int UpdateEmployee(Guid employeeID, Employee employee);


        /// <summary>
        /// API lấy mã nhân viên lớn nhất trong database
        /// </summary>
        /// <returns>mã nhân viên lớn nhất</returns>
        /// CreatedBy: VDTIEN (14/11/2022)
        public string GetEmployeeCodeMax();
    } 

}
