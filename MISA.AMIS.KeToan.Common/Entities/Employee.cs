using MISA.AMIS.KeToan.Common.Attributes;
using MISA.AMIS.KeToan.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace MISA.AMIS.KeToan.Common.Entities
{
    /// <summary>
    /// Nhân viên
    /// </summary>
    public class Employee : BaseEntity
    {
        #region Field

        #endregion

        #region Propety
        /// <summary>
        /// ID nhân viên
        /// </summary>
        [Key]
        public Guid EmployeeID { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [NotNullOrNotEmpty(ErrorMessage = "Mã nhân viên không được để trống không được để trống")]
        public string? EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary> 
        [NotNullOrNotEmpty(ErrorMessage = "Tên nhân viên không được để trống không được để trống")]
        public string? EmployeeName { get; set; }
        
        /// <summary>
        /// Giới tính
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        [ErrorDate(ErrorMessage = "Ngày sinh không lớn hơn ngày hiện tại")]
       public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// id phòng bàn
        /// </summary>
        [NotNullOrNotEmpty(ErrorMessage = "Đơn vị không được để trống không được để trống")]
        public Guid? DepartmentID { get; set; }

        /// <summary>
        /// id phòng bàn
        /// </summary>
        
        public string? DepartmentName { get; set; }
        /// <summary>
        /// tên chức danh
        /// </summary>
        public string? JobPositionName { get; set; }

        /// <summary>
        /// Số CMND
        /// </summary>
        public string? IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp CMND
        /// </summary>
        [ErrorDate(ErrorMessage = "Ngày cấp không lớn hơn ngày hiện tại")]
        public DateTime? IdentityDate { get; set; }
        
        /// <summary>
        /// Nơi cấp CMND
        /// </summary>
        public string? IdentityPlace { get; set; }
        
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// số điện thoại cố định
        /// </summary>
        public string? TelephoneNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string? BankName { get; set; }

        /// <summary>
        /// Số TK
        /// </summary>
        public string? BankAccountNumber { get; set; }

        /// <summary>
        /// Chi nhánh ngân hàng
        /// </summary>
        public string? BankBranchName { get; set; }

        #endregion

        #region Constructor

        #endregion

        #region Method

        #endregion

        #region Override
        #endregion
    }
}
