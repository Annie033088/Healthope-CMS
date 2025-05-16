using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.Coach.Response
{
    public class ResponseGetCoachListDto
    {
        /// <summary>
        /// 教練清單
        /// </summary>
        public List<ResponseGetCoachDto> CoachList { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }
    }

    public class ResponseGetCoachDto
    {
        /// <summary>
        /// 教練 ID，主鍵
        /// </summary>
        public int CoachId { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        public int Phone { get; set; }

        /// <summary>
        /// 教練姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 教練類型（自訂分類代碼）
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 合約起始日期
        /// </summary>
        public DateTime ContractStartTime { get; set; }

        /// <summary>
        /// 合約結束日期
        /// </summary>
        public DateTime ContractEndTime { get; set; }
    }
}