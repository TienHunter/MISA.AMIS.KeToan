using Dapper;
using MISA.AMIS.KeToan.Common;
using MISA.AMIS.KeToan.Common.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.DL
{
    public class BaseDL<T> : IBaseDL<T>
    {

        #region Field
        // Khởi tạo thamo số kết nối tới DB MySSQL
        private string connectionString = DatabaseContext.ConnectionString;

        #endregion

        #region Methods
        /// <summary>
        /// Lấy danh sách tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách tất cả bản ghi</returns>
        /// Created by: VDTien(10/11/2022)
        public IEnumerable<T> GetAllRecords()
        {



            // Chuẩn bị câu lệnh SQL
            string storedProcedureName = String.Format(Procedure.GET_ALL, typeof(T).Name);

            // Chuẩn bị tham số đầu vào

            // Thực hiện gọi vào DB
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {

                var records = mySqlConnection.Query<T>(sql: storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);
                return records;
            }
            // Xử lý kết quả trả về 

        }

        /// <summary>
        /// Lấy thông tin 1 bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID của bản ghi muốn lấy</param>
        /// <returns>Thông tin của 1 bản ghi</returns>
        /// Created by: VDTien(10/11/2022)
        public T GetRecordByID(Guid recordID)
        {

            //Khởi tạo kết nối tới DB MySQL

            //Chuẩn bị câu lệnh SQL
            string storedProcedureName = String.Format(Procedure.GET_BY_ID, typeof(T).Name);

            //Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"@{typeof(T).Name}ID", recordID);

            //Thực hiện gọi vào DB
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {

                var record = mySqlConnection.QueryFirstOrDefault<T>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return record;
            }

            ////Xử lý kết quả trả về
            //return null;
        }




        #endregion

    }
}
