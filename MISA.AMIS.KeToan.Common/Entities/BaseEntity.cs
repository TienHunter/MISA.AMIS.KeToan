using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.Common.Entities
{
    public class BaseEntity
    {
        #region Property

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// NNgười tạo
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Thời gian chỉnh sửa gần nhất
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Thời gian chỉnh sửa gần nhất
        /// </summary>
        public string UpdatedBy { get; set; }

        #endregion
    }
}
