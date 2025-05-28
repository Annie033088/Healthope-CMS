using System;

namespace DomainLayer.Models
{
    public class MembershipPlan
    {
        /// <summary>
        /// 會籍方案 Id
        /// </summary>
        public int MembershipPlanId { get; set; }

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
        /// 照片路徑
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 是否顯示在前台
        /// </summary>
        public bool Display { get; set; }

        /// <summary>
        /// 狀態 (有/無效)
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
