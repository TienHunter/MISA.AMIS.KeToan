using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.Common.Entities.DTO
{
    public class ActionResult
    {
        #region Property

        /// <summary>
        /// số bản ghi bị ảnh hưởng khi truy vấn
        /// </summary>
        public int numberOfRowsAffected { get; set; }

        /// <summary>
        /// ID nhân viên được truy vấn
        /// </summary>
        public Guid EmployeeID { get; set; }

        #endregion
    }
}
