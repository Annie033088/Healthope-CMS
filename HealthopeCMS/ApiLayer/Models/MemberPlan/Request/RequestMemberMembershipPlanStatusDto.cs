using System;

namespace ApiLayer.Models.MemberPlan.Request
{
    public class RequestMemberMembershipPlanStatusDto
    {
        /// <summary>
        /// 會員的會籍方案 Id
        /// </summary>
        public int MemberMembershipPlanId { get; set; }

        /// <summary>
        /// 狀態 1:未啟用 ; 2:進行中 ; 3:終止 ; 4:暫停 ; 5:完成
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime UpdateTime { get; set; }

    }
}