namespace PersistentLayer.Models
{
    public class RequestGetInvoiceDto
    {
        /// <summary>
        /// 狀態 1.處理中 2.成功 3.失敗 4.待作廢 5.已作廢 6.待折讓 7.已折讓
        /// </summary>
        public byte? Status { get; set; }

        /// <summary>
        /// 分類 1:主發票(一般銷售) 2:違約金發票
        /// </summary>
        public byte? Category { get; set; }

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
