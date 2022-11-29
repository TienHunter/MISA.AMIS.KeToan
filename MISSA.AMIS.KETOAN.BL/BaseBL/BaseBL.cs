using MISA.AMIS.KeToan.Common.Attributes;
using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.DL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.BL
{
    public class BaseBL<T> : IBaseBL<T>
    {


        #region Field
        private IBaseDL<T> _baseDL;
        #endregion

        #region Constructor
        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }

        #endregion


        #region Method
        /// <summary>
        /// Lấy danh sách tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách tất cả bản ghi</returns>
        /// Created by: VDTien(10/11/2022)
        public IEnumerable<T> GetAllRecords()
        {
            return _baseDL.GetAllRecords();
        }


        /// <summary>
        /// Lấy thông tin 1 bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID của bản ghi muốn lấy</param>
        /// <returns>Thông tin của 1 bản ghi</returns>
        /// Created by: VDTien(10/11/2022)
        public T GetRecordByID(Guid recordID)
        {
            return _baseDL.GetRecordByID(recordID);
        }


        public virtual Dictionary<string, string> ValidateInput(T entity)
        {
            var props = entity.GetType().GetProperties();

            // lấy ra tất cả các property được đánh dấu không được phép để trống
            var propNotEmpties = props.Where(prop=>Attribute.IsDefined(prop,typeof(NotNullOrNotEmpty)));
            var errorMore = new Dictionary<string, string>();
            foreach (var prop in propNotEmpties)
            {
                var propValue = prop.GetValue(entity);
                var propName = prop.Name;
                var notNullOrNotEmpty = (NotNullOrNotEmpty?)Attribute.GetCustomAttribute(prop, typeof(NotNullOrNotEmpty));
                if (propValue == null || string.IsNullOrEmpty(propValue?.ToString() )) {
                    errorMore.Add(propName, notNullOrNotEmpty.ErrorMessage);
                }
            }

            // lấy ra tất cả các property được đánh dấu không được phép để trống
            var properrorDates = props.Where(prop => Attribute.IsDefined(prop, typeof(ErrorDate)));
            foreach (var prop in properrorDates)
            {
                var propValue = prop.GetValue(entity);
                var propName = prop.Name;
                var errorDate = (ErrorDate?)Attribute.GetCustomAttribute(prop, typeof(ErrorDate));
                if (propValue != null && (DateTime)propValue > DateTime.Now)
                {
                    errorMore.Add(propName, errorDate.ErrorMessage);
                }
            }

            return errorMore;
        }
        #endregion

    }
}
