using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.Invoice.Request
{
    public class RequestOrderIdAndCategoryDto
    {
        /// <summary>
        /// 訂單 Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 分類 (1:主發票 2:違約金發票)
        /// </summary>
        public byte Category { get; set; }
    }
}