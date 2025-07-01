using System;

namespace DomainLayer.Models
{
    public class OrderState
    {
        /// <summary>
        /// 訂單狀態 Id
        /// </summary>
        public int OrderStateId { get; set; }

        /// <summary>
        /// 訂單 Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 訂單狀態
        /// </summary>
        public byte State { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }

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
