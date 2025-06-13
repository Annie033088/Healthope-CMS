namespace ApiLayer.Models.Invoice.Request
{
    public class RequestAddInvoiceTrackNumberDto
    {
        /// <summary>
        /// 發票號碼前兩碼
        /// </summary>
        public string TrackPrefix { get; set; }

        /// <summary>
        /// 起始碼
        /// </summary>
        public int StartNumber { get; set; }

        /// <summary>
        /// 結束碼
        /// </summary>
        public int EndNumber { get; set; }

        /// <summary>
        /// 哪年哪期的發票 (EX:1141(114年1期), 1142...)
        /// </summary>
        public int InvoicePeriod { get; set; }
    }
}