namespace PersistentLayer.Models
{
    public class RequestGetRefundDto
    {
        /// <summary>
        /// 篩選狀態
        /// </summary>
        public byte? Status { get; set; }

        /// <summary>
        /// 退款類型
        /// </summary>
        public byte? RefundType { get; set; }

        /// <summary>
        /// 升/降序
        /// </summary>
        public string SortOrder { get; set; }

        /// <summary>
        /// 排序選項
        /// </summary>
        public string SortOption { get; set; }

        /// <summary>
        /// 一頁顯示 x 筆
        /// </summary>
        public int RecordPerPage { get; set; }

        /// <summary>
        /// 搜尋的頁數
        /// </summary>
        public int Page { get; set; }
    }
}
