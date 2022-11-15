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
        /// Format tên của Procedure lấy 1 bản ghi theo id
        /// </summary>
        public static string GET_BY_ID = "Proc_{0}_GetById";
    }
}
