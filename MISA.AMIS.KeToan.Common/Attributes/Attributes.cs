using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.Common.Attributes
{

    [AttributeUsage(AttributeTargets.Property)]
    public class NotNullOrNotEmpty:Attribute
    {
        public string ErrorMessage { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ErrorDate : Attribute
    {
        public string ErrorMessage { get; set; }
    }

}
