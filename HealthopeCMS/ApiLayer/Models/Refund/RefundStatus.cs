using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.Refund
{
    public enum RefundStatus : byte
    {
        /// <summary>
        /// 待處理
        /// </summary>
        Pending = 1,

        /// <summary>
        /// 已處理
        /// </summary>
        Processed = 2,

        /// <summary>
        /// 失敗
        /// </summary>
        Fail = 3,
    }
}