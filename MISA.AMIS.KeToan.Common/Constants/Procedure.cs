using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.Common
{
    public class Procedure
    {
        /// <summary>
        /// Format tên của Procedure láy tất cả bản ghi
        /// </summary>
        public static string GET_ALL = "Proc_{0}_GetAll";

        /// <summary>
        /// Format tên của Procedure láy tất cả bản ghi theo bộ lọc
        /// </summary>
        public static string GET_FILTER = "Proc_{0}_GetByFilter";

        /// <summary>
        /// Format tên của Procedure lấy 1 bản ghi theo id
        /// </summary>
        public static string GET_BY_ID = "Proc_{0}_GetById";

        /// <summary>
        /// Procedure lấy danh sách nhân viên theo lọc và phân trang
        /// </summary>
        public static string GET_EMPLOYEES_BY_FILTER_PAGING = "Proc_employee_Pagination";

        /// <summary>
        /// Procedure thêm mới 1 nhân viên
        /// </summary>
        public static string INSERT_EMPLOYEE = "Proc_employee_Insert";

        /// <summary>
        /// Procedure xóa 1 nhân viên theo id
        /// </summary>
        public static string DELETE_EMPLOYEE_BY_ID = "Proc_employee_DeleteByID";

        /// <summary>
        /// Procedure cập nhật nhân viên theo id
        /// </summary>
        public static string UPDATE_EMPLOYEE_BY_ID = "Proc_employee_UpdateById";

        /// <summary>
        /// Procdure xóa nhân viên hàng loạt
        /// </summary>
        public static string DELETE_BATCH_EMPLOYEE = "Proc_employee_deleteMultiEmployee";
        
        /// <summary>
        /// Procedure trả về mã số nhân viên lớn nhất
        /// </summary>
        public static string GET_EMPLOYEE_CODE_MAX = "Proc_employee_GetEmployeeCodeMax";

        /// <summary>
        /// Procedure trả về mã nhân viên trùng
        /// </summary>
        public static string CHECK_DUPLICATE_EMPLOYEE_CODE = "Proc_employee_checkDuplicateEmployeeCode";

        /// <summary>
        /// Procedure trả về danh sách nhân viên theo bộ lọc
        /// </summary>
        public static string GET_EMPLOYEES_BY_FILTER = "Proc_employee_GetByFilter";
    }
}
