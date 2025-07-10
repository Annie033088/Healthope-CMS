namespace ApiLayer.Models.Refund
{
    public enum RefundType
    {
        /// <summary>
        /// 解約
        /// </summary>
        Terminate = 1,

        /// <summary>
        /// 違約
        /// </summary>
        Breach = 2,

        /// <summary>
        /// 7 日內退款
        /// </summary>
        RefundIn7Days = 2,
    }
}