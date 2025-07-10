using System;

namespace ApiLayer.Models.Invoice.Request
{
    public class RequestEditInvoiceStatusDto
    {
        /// <summary>
        /// 訂單 Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 分類 (1:主發票 2:違約金發票)
        /// </summary>
        public byte Category { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}