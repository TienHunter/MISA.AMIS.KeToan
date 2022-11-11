using Dapper;
using MISA.AMIS.KeToan.Common.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.DL
{
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {

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
        public IEnumerable<dynamic> GetEmployeesByFilterAndPaging(string keyWord, int limit, int offset, string sort)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     API thêm mới 1 nhân viên
        /// </summary>
        /// <param name="employee">Đối tượng nhân viên cần thêm mới</param>
        /// <returns>ID nhân viên vừa thêm mới</returns>
        /// Created by: VDTien (01/11/2022)
        public int InsertEmployee(Employee employee)
        {

            //Khởi tạo kết nối tới DB MySQL
            string connectionString = "Server=localhost;Port=3306;Database=misa.web09.ctm.vdtien;Uid=root;Pwd=tien.hust;";
            var mySqlConnection = new MySqlConnection(connectionString);

            //Chuẩn bị câu lệnh SQL
            string storedProcedureName = "Proc_employee_Insert";

            //Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            var newEmployeeID = Guid.NewGuid();
            parameters.Add("@EmployeeID", newEmployeeID);
            parameters.Add("@EmployeeCode", employee.EmployeeCode);
            parameters.Add("@EmployeeName", employee.EmployeeName);
            parameters.Add("@DepartmentID", employee.DepartmentID);
            parameters.Add("@JobPositionName", employee.JobPositionName);
            parameters.Add("@Gender", employee.Gender);
            parameters.Add("@DateofBirth", employee.DateOfBirth);
            parameters.Add("@IdentityNumber", employee.IdentityNumber);
            parameters.Add("@IdentityDate", employee.IdentityDate);
            parameters.Add("@IdentityPlace", employee.IdentityPlace);
            parameters.Add("@Address", employee.Address);
            parameters.Add("@PhoneNumber", employee.PhoneNumber);
            parameters.Add("@TelephoneNumber", employee.TelephoneNumber);
            parameters.Add("@Email", employee.Email);
            parameters.Add("@BankAccountNumber", employee.BankAccountNumber);
            parameters.Add("@BankName", employee.BankName);
            parameters.Add("@BankBranchName", employee.BankBranchName);
            parameters.Add("@CreatedDate", employee.CreatedDate);
            parameters.Add("@CreatedBy", employee.CreatedBy);
            parameters.Add("@UpdatedDate", employee.UpdatedDate);
            parameters.Add("@UpdatedBy", employee.UpdatedBy);


            //Thực hiện gọi vào DB
            int numberOfRowsAffected = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            //Xử lý kết quả trả về
            // numberOfRowsAffected luôn trả về 0 ??
            return numberOfRowsAffected;
        }

        /// <summary>
        /// API sửa thông tin nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID nhân viên muốn sửa</param>
        /// <param name="employee">Đối tượng nhân viên muốn sửa</param>
        /// <returns>ID của nhân viên vừa sửa</returns>
        /// CreatedBy: VDTien (1/11/2022)
        public int UpdateEmployee(Guid employeeID, Employee employee)
        {
            //Khởi tạo kết nối tới DB MySQL
            string connectionString = "Server=localhost;Port=3306;Database=misa.web09.ctm.vdtien;Uid=root;Pwd=tien.hust;";
            var mySqlConnection = new MySqlConnection(connectionString);

            //Chuẩn bị câu lệnh SQL
            string storedProcedureName = "Pro_employee_UpdateById";

            //Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add("@EmployeeID", employeeID);
            parameters.Add("@EmployeeCode", employee.EmployeeCode);
            parameters.Add("@EmployeeName", employee.EmployeeName);
            parameters.Add("@DepartmentID", employee.DepartmentID);
            parameters.Add("@JobPositionName", employee.JobPositionName);
            parameters.Add("@Gender", employee.Gender);
            parameters.Add("@DateOfBirth", employee.DateOfBirth);
            parameters.Add("@IdetityNumber", employee.IdentityNumber);
            parameters.Add("@IdetityDate", employee.IdentityDate);
            parameters.Add("@IdentityPlace", employee.IdentityPlace);
            parameters.Add("@Address", employee.Address);
            parameters.Add("@PhoneNumber", employee.PhoneNumber);
            parameters.Add("@TelephoneNumber", employee.TelephoneNumber);
            parameters.Add("@Email", employee.Email);
            parameters.Add("@BankAccountNumber", employee.BankAccountNumber);
            parameters.Add("@BankName", employee.BankName);
            parameters.Add("@BankBranchName", employee.BankBranchName);
            parameters.Add("@UpdatedBy", employee.UpdatedBy);


            //Thực hiện gọi vào DB
            int numberOfRowsAffected = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            return numberOfRowsAffected;
        }

        /// <summary>
        /// API xóa thông tin nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID nhân viên muốn xóa</param>
        /// <returns>ID nhân viên vừa xóa</returns>
        /// CreatedBy: VDTien (1/11/2022)
        public Guid DeleteEmployeeByID(Guid employeeID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// API xóa nhiều nhân viên theo danh sách ID
        /// </summary>
        /// <param name="listEmployeeID">danh sách id nhân viên muốn xóa</param>
        /// <returns>StatusCode200</returns>
        /// CreatedByL VDTIEN(1/11/2022)
        public int DeleteMultipleEmployeesByID(List<Guid> listEmployeeID)
        {
            throw new NotImplementedException();
        }
    }
}
