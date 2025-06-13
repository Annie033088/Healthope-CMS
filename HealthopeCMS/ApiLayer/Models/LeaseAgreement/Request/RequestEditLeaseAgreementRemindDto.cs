using System;

namespace ApiLayer.Models.LeaseAgreement.Request
{
    public class RequestEditLeaseAgreementRemindDto
    {
        /// <summary>
        /// 租約 Id
        /// </summary>
        public int LeaseAgreementId { get; set; }

        /// <summary>
        /// 提醒是否開啟
        /// </summary>
        public bool Remind { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }

    }
}