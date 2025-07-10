namespace ApiLayer.Models.MemberPlan
{
    public enum MemberMembershipPlanStatus : byte
    {
        /// <summary>
        /// 未啟用
        /// </summary>
        Inactive = 1,

        /// <summary>
        /// 進行中
        /// </summary>
        Active = 2,

        /// <summary>
        /// 終止
        /// </summary>
        Terminated = 3,

        /// <summary>
        /// 暫停
        /// </summary>
        Paused = 4,

        /// <summary>
        /// 完成
        /// </summary>
        Completed = 5,
    }
}