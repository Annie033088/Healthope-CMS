using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.Invoice
{
    public enum InvoiceTrackNumberStatus : byte
    {
        /// <summary>
        /// 未啟用
        /// </summary>
        Inactive = 1,

        /// <summary>
        /// 啟用中
        /// </summary>
        Active = 2,

        /// <summary>
        /// 已停用
        /// </summary>
        Disabled = 3,

        /// <summary>
        /// 結束
        /// </summary>
        Closed = 4,
    }
}