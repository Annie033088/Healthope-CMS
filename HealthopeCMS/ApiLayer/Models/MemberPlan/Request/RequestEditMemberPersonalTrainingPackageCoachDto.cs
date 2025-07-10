using System;

namespace ApiLayer.Models.MemberPlan.Request
{
    public class RequestEditMemberPersonalTrainingPackageCoachDto
    {
        /// <summary>
        /// 會員的教練課方案 Id
        /// </summary>
        public int MemberPersonalTrainingPackageId { get; set; }

        /// <summary>
        /// 教練 Id
        /// </summary>
        public int CoachId { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}