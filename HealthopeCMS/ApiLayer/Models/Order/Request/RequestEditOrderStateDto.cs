using System;

namespace ApiLayer.Models.Order.Request
{
    public class RequestEditOrderStateDto
    {
        /// <summary>
        /// 訂單 Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}