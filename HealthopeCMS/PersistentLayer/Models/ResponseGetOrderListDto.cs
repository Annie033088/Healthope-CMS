using System;
using System.Collections.Generic;

namespace ApiLayer.Models.Order.Response
{
    public class ResponseGetOrderListDto
    {
        /// <summary>
        /// 訂單清單
        /// </summary>
        public List<ResponseGetOrderDto> OrderList { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }
    }

    public class ResponseGetOrderDto
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
        /// 會員 名稱
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 會員手機
        /// </summary>
        public int MemberPhone { get; set; }

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
        /// 發票狀態
        /// </summary>
        public byte InvoiceStatus { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}