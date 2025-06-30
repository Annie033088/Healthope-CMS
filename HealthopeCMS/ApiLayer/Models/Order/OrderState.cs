using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.Order
{
    public enum OrderState : byte
    {
        /// <summary>
        /// 待付款
        /// </summary>
        Pending = 1,

        /// <summary>
        /// 已付款
        /// </summary>
        Paid = 2,

        /// <summary>
        /// 取消
        /// </summary>
        Cancel = 3,

        /// <summary>
        /// 違約取消
        /// </summary>
        TerminatedWithPenalty = 4,

        /// <summary>
        /// 已部分退款
        /// </summary>
        PartiallyRefunded = 5,

        /// <summary>
        /// 付款處理中
        /// </summary>
        Paying = 6,
    }
}