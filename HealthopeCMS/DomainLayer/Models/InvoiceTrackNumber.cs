using System;

namespace DomainLayer.Models
{
    public class InvoiceTrackNumber
    {
        /// <summary>
        /// 字軌 Id
        /// </summary>
        public int InvoiceTrackNumberId { get; set; }

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
        /// 當前已使用號碼
        /// </summary>
        public int CurrentNumber { get; set; }

        /// <summary>
        /// 哪年哪期的發票 (EX:1141(114年1期), 1142...)
        /// </summary>
        public int InvoicePeriod { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
