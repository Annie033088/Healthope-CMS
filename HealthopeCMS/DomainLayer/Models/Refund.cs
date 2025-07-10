using System;

namespace DomainLayer.Models
{
    public class Refund
    {
        /// <summary>
        /// 退費 Id
        /// </summary>
        public int RefundId { get; set; }

        /// <summary>
        /// 訂單 Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 電子發票 Id
        /// </summary>
        public int ElectronicInvoiceId { get; set; }

        /// <summary>
        /// 退款類型
        /// </summary>
        public byte RefundType { get; set; }

        /// <summary>
        /// 狀態 1.待處理 2.已處理 3.失敗
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 退費金額 原始應退
        /// </summary>
        public int RefundAmount { get; set; }

        /// <summary>
        /// 違約金
        /// </summary>
        public int PenaltyAmount { get; set; }

        /// <summary>
        /// 創建時間(退費時間)
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
