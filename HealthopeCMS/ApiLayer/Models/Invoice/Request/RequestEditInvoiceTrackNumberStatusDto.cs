using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.Invoice.Request
{
    public class RequestEditInvoiceTrackNumberStatusDto
    {
        /// <summary>
        /// 字軌 Id
        /// </summary>
        public int InvoiceTrackNumberId { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}