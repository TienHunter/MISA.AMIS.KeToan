namespace MISA.AMIS.KeToan.Common.Entities
{
    /// <summary>
    /// Kết quả trả về của API lấy danh sách nhân viên theo bộ lọc và phân trang
    /// </summary>
    public class PagingResult
    {
        /// <summary>
        /// Danh sách nhân viên
        /// </summary>
        public List<Employee> Data { get; set; }

        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        public long TotalRecords { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public long TotalPages { get; set; }
    }
}
