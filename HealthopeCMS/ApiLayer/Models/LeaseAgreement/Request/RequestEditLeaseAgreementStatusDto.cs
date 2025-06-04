using System;

namespace ApiLayer.Models.LeaseAgreement.Request
{
    public class RequestEditLeaseAgreementStatusDto
    {
        /// <summary>
        /// 租約 Id
        /// </summary>
        public int LeaseAgreementId { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public byte Status { get; set; }

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