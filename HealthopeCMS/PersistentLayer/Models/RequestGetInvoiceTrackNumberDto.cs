namespace PersistentLayer.Models
{
    public class RequestGetInvoiceTrackNumberDto
    {
        /// <summary>
        /// 狀態
        /// </summary>
        public byte? Status { get; set; }

        /// <summary>
        /// 過期/未過期
        /// </summary>
        public bool? Time { get; set; }

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
