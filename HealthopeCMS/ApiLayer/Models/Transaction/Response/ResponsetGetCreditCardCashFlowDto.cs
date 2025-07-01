namespace ApiLayer.Models.Transaction.Response
{
    public class ResponsetGetCreditCardCashFlowDto
    {
        /// <summary>
        /// 授權碼
        /// </summary>
        public string AuthCode { get; set; }

        /// <summary>
        /// 外部平台的交易 Id
        /// </summary>
        public string GatewayTransactionId { get; set; }
    }
}