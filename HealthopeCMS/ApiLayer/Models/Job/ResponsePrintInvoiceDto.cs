namespace ApiLayer.Models.Job
{
    public class ResponsePrintInvoiceDto
    {
        /// <summary>
        /// 回傳 成功/失敗 code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 成功會回傳開立號碼
        /// </summary>
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// 開立時間
        /// </summary>
        public string InoviceTime { get; set; }

        /// <summary>
        /// 隨機 4 碼
        /// </summary>
        public string RandomNumber { get; set; }
    }
}