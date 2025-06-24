using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.Order.Response
{
    public class ResponseGetOrderDetailByIdDto
    {
        /// <summary>
        /// 訂單細項
        /// </summary>
        public ResponseGetOrderByIdDto Order { get; set; }

        /// <summary>
        /// 訂單狀態列表
        /// </summary>
        public List<ResponseGetOrderStateByIdDto> OrderStateList { get; set; }
    }

    public class ResponseGetOrderByIdDto
    {
        /// <summary>
        /// 方案名
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public long OrderNumber { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 付款方式 (1.現金 ; 2.信用卡)
        /// </summary>
        public byte Method { get; set; }

        /// <summary>
        /// 狀態
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
    }

    public class ResponseGetOrderStateByIdDto
    {
        /// <summary>
        /// 訂單狀態 Id
        /// </summary>
        public int OrderStateId { get; set; }

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