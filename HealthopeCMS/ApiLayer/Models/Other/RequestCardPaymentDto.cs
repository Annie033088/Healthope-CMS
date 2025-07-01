namespace ApiLayer.Models.Other
{
    public class RequestCardPaymentDto
    {
        /// <summary>
        /// 訂單 Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 交易 Id
        /// </summary>
        public string TransactionId { get; set; }
    }
}