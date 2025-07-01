using System;

namespace PersistentLayer.Models
{
    public class RequestPayByCardDto
    {
        /// <summary>
        /// 訂單 Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 教練 ID，主鍵
        /// </summary>
        public int? CoachId { get; set; }

        /// <summary>
        /// 刷卡機 Id
        /// </summary>
        public string CardReaderId { get; set; }
    }
}
