using System;

namespace DomainLayer.Models
{
    public class PaymentTransaction
    {
        /// <summary>
        /// 交易 Id
        /// </summary>
        public int TransactionId { get; set; }

        /// <summary>
        /// 訂單 Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 付款(交易)方式 1:現金 ; 2:信用卡
        /// </summary>
        public byte Method { get; set; }

        /// <summary>
        /// 金額 
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 狀態 1.處理中 2.成功 3.失敗
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 授權碼
        /// </summary>
        public string AuthCode { get; set; }

        /// <summary>
        /// 交易卡片末四碼
        /// </summary>
        public string CardLastFour { get; set; }

        /// <summary>
        /// 卡片類型(EX: VISA ; MasterCard ; JCB)
        /// </summary>
        public string CardType { get; set; }

        /// <summary>
        /// 外部平台的交易 Id
        /// </summary>
        public string GatewayTransactionId { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime Time { get; set; }
    }
}
