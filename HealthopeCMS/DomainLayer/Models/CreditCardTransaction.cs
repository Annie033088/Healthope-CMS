using System;

namespace DomainLayer.Models
{
    public class CreditCardTransaction
    {
        /// <summary>
        /// 信用卡交易紀錄 Id
        /// </summary>
        public int CreditCardTransactionId { get; set; }

        /// <summary>
        /// 訂單 Id
        /// </summary>
        public int OrderId { get; set; }

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
        /// 金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
