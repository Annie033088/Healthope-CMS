namespace ApiLayer.Models.Other
{
    public class ResponseCardPaymentDto
    {
        /// <summary>
        /// 驗證碼
        /// </summary>
        public string AuthCode { get; set; }

        /// <summary>
        /// 卡片末四碼
        /// </summary>
        public string CardLastFour { get; set; }

        /// <summary>
        /// 卡片類型 (Visa/Master...
        /// </summary>
        public string CardType { get; set; }

        /// <summary>
        /// 金流交易 Id
        /// </summary>
        public string TransactionId { set; get; }

        /// <summary>
        /// 成功/失敗
        /// </summary>
        public bool Status { get; set; }
    }
}