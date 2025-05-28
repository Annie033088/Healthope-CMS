using System;
using System.Collections.Generic;

namespace ApiLayer.Models.PlanTemplate.Response
{
    public class ResponseGetTicketPlanListDto
    {
        /// <summary>
        /// 教練課方案清單
        /// </summary>
        public List<ResponseGetTicketPlanDto> TicketPlanList { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }
    }

    public class ResponseGetTicketPlanDto
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