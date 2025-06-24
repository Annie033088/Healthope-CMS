using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.Order.Request
{
    public class RequestEditOrderStateRemarkDto
    {
        /// <summary>
        /// 訂單狀態 Id
        /// </summary>
        public int OrderStateId { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}