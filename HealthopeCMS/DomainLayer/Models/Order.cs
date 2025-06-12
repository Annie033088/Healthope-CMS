using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class Order
    {
        /// <summary>
        /// 訂單 Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 會員 Id
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 方案編號(EX:教練課/票劵/會籍 的方案Id)
        /// </summary>
        public int PlanId { get; set; }

        /// <summary>
        /// 方案類別
        /// </summary>
        public byte PlanType { get; set; }

        /// <summary>
        /// 方案名
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public long OrderNumber { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public byte State { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 付款方式 (1.現金 ; 2.信用卡)
        /// </summary>
        public byte Method { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
