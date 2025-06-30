using System;

namespace DomainLayer.Models
{
    public class ElectronicInvoice
    {
        /// <summary>
        /// 電子發票 Id
        /// </summary>
        public int ElectronicInvoiceId { get; set; }

        /// <summary>
        /// 訂單 Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 發票號碼
        /// </summary>
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// 開立時間
        /// </summary>
        public DateTime InvoiceTime { get; set; }

        /// <summary>
        /// 隨機碼
        /// </summary>
        public string RandomNumber { get; set; }

        /// <summary>
        /// 買家統編(個人 00000000)
        /// </summary>
        public string Buyer { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        public int TotalAmount { get; set; }

        /// <summary>
        /// B2C/B2B (預設目前都是 B2C)
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 分類 (1:主發票 2:違約金發票)
        /// </summary>
        public byte Category { get; set; }

        /// <summary>
        /// 開立狀態 (1:處理中 2:成功 3:失敗)
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
