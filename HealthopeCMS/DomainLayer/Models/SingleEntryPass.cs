using System;

namespace DomainLayer.Models
{
    public class SingleEntryPass
    {
        /// <summary>
        /// 一次性入場劵 Id
        /// </summary>
        public int SingleEntryPassId { get; set; }

        /// <summary>
        /// 訂單 Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 隨機碼
        /// </summary>
        public Guid TicketCode { get; set; }

        /// <summary>
        /// 過期時間
        /// </summary>
        public DateTime Expiry { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
