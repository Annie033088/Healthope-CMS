using System;
using System.Collections.Generic;

namespace PersistentLayer.Models
{
    public class ResponseGetTransactionListDto
    {
        /// <summary>
        /// 付款紀錄清單
        /// </summary>
        public List<ResponseGetTransactionDto> TransactionList { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }
    }

    public class ResponseGetTransactionDto
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
        /// 創建時間
        /// </summary>
        public DateTime Time { get; set; }
    }
}