using System;

namespace DomainLayer.Models
{
    public class TicketPlan
    {
        /// <summary>
        /// 一次性票劵 Id 
        /// </summary>
        public int TicketPlanId { get; set; }

        /// <summary>
        /// 價格
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// 狀態 (有/無效)
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
