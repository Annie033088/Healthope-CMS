using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.Order
{
    public enum PaymentMethod : byte
    {
        /// <summary>
        /// 現金
        /// </summary>
        Cash = 1,

        /// <summary>
        /// 刷卡
        /// </summary>
        Card = 2
    }
}