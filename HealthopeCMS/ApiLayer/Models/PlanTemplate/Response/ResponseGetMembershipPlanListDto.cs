using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.Response.PlanTemplate
{
    public class ResponseGetMembershipPlanListDto
    {
        /// <summary>
        /// 會籍方案清單
        /// </summary>
        public List<ResponseGetMembershipPlanDto> MembershipPlanList { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }
    }

    public class ResponseGetMembershipPlanDto
    {
        /// <summary>
        /// 會籍方案 Id
        /// </summary>
        public int membershipPlanId { get; set; }

        /// <summary>
        /// 方案名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 價格
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// 期限(單位:月)
        /// </summary>
        public byte Duration { get; set; }

        /// <summary>
        /// 介紹
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 是否顯示在前台
        /// </summary>
        public bool Display { get; set; }

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