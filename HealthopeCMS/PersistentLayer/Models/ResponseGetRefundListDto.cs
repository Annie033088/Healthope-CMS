using System;
using System.Collections.Generic;

namespace PersistentLayer.Models
{
    public class ResponseGetRefundListDto
    {
        /// <summary>
        /// 退款清單
        /// </summary>
        public List<ResponseGetRefundDto> RefundList { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }
    }

    public class ResponseGetRefundDto
    {
        /// <summary>
        /// 退費 Id
        /// </summary>
        public int RefundId { get; set; }

        /// <summary>
        /// 訂單 Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 會員 Id
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 會員名稱
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 會員手機
        /// </summary>
        public int MemberPhone { get; set; }

        /// <summary>
        /// 退款類型
        /// </summary>
        public byte RefundType { get; set; }

        /// <summary>
        /// 狀態 1.待處理 2.已處理 3.失敗
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 退費金額 原始應退
        /// </summary>
        public int RefundAmount { get; set; }

        /// <summary>
        /// 違約金
        /// </summary>
        public int PenaltyAmount { get; set; }

        /// <summary>
        /// 創建時間(退費時間)
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}