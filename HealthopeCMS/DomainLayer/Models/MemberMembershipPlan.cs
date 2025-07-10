using System;

namespace DomainLayer.Models
{
    public class MemberMembershipPlan
    {
        /// <summary>
        /// 會員的會籍方案 Id
        /// </summary>
        public int MemberMembershipPlanId { get; set; }

        /// <summary>
        /// 訂單 Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 會員 Id
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 方案名
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// 時限
        /// </summary>
        public byte Duration { get; set; }

        /// <summary>
        /// 狀態 1:未啟用 ; 2:進行中 ; 3:終止 ; 4:暫停 ; 5:完成
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
