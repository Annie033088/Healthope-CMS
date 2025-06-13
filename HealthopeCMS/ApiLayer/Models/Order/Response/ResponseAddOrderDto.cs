using System;

namespace ApiLayer.Models.Order.Response
{
    public class ResponseAddOrderDto
    {
        /// <summary>
        /// 訂單 Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}