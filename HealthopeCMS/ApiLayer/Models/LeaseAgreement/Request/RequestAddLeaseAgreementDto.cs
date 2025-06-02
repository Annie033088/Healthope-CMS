using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.LeaseAgreement.Request
{
    public class RequestAddLeaseAgreementDto
    {
        /// <summary>
        /// 開始日
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 結束日
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 提醒前置天數
        /// </summary>
        public int ReminderLeadTime { get; set; }
    }
}