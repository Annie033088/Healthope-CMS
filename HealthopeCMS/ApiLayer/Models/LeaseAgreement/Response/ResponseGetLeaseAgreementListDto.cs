using System;
using System.Collections.Generic;

namespace ApiLayer.Models.LeaseAgreement.Response
{
    public class ResponseGetLeaseAgreementListDto
    {
        /// <summary>
        /// 租約清單
        /// </summary>
        public List<ResponseGetLeaseAgreementDto> LeaseAgreementList { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }
    }

    public class ResponseGetLeaseAgreementDto
    {
        /// <summary>
        /// 租約 Id
        /// </summary>
        public int LeaseAgreementId { get; set; }

        /// <summary>
        /// 開始日
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 結束日
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 提醒是否開啟
        /// </summary>
        public bool Remind { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 提醒前置天數
        /// </summary>
        public int ReminderLeadTime { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}