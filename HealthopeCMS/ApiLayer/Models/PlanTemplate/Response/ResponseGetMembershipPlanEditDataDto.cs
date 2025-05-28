using System;

namespace ApiLayer.Models.PlanTemplate.Response
{
    public class ResponseGetMembershipPlanEditDataDto
    {
        /// <summary>
        /// 方案名
        /// </summary>
        public string Name { get; set; }

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
        /// 照片路徑
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}