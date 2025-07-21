namespace ApiLayer.Models.MemberClass
{
    public enum MemberPersonalClassStatus : byte
    {
        /// <summary>
        /// 預約中
        /// </summary>
        BookingInProgress = 1,

        /// <summary>
        /// 預約成功
        /// </summary>
        BookedSuccessfully = 2,

        /// <summary>
        /// 未出席
        /// </summary>
        DidNotAttend = 3,

        /// <summary>
        /// 已出席
        /// </summary>
        Attended = 4,

        /// <summary>
        /// 取消
        /// </summary>
        Cancelled = 5,
    }
}