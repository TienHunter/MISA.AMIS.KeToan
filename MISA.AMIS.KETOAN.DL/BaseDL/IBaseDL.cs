using MISA.AMIS.KeToan.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.DL
{
    public interface IBaseDL<T>
    {
        /// <summary>
        /// Lấy danh sách tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách tất cả bản ghi</returns>
        /// Created by: VDTien(10/11/2022)
        public IEnumerable<T> GetAllRecords();

        /// <summary>
        /// Lấy thông tin 1 bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID của bản ghi muốn lấy</param>
        /// <returns>Thông tin của 1 bản ghi</returns>
        /// Created by: VDTien(10/11/2022)
        public T GetRecordByID(Guid recordID);

    }
}
