using MISA.AMIS.KeToan.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.Common.Exceptions
{
    public class ValidateException : Exception
    {
        public ValidateException(Dictionary<string, string> errorMore)
        {
            this.errorMore = errorMore;
        }
        public Dictionary<string, string> errorMore { get; set; }
    }
}
