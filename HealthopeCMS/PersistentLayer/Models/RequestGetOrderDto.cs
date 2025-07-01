namespace PersistentLayer.Models
{
    public class RequestGetOrderDto
    {
        /// <summary>
        /// 篩選狀態
        /// </summary>
        public byte? State { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public byte? Method { get; set; }

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
