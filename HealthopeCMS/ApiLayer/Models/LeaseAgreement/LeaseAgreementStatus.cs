using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.LeaseAgreement
{
    public enum LeaseAgreementStatus : byte
    {
        /// <summary>
        /// 未啟用
        /// </summary>
        Inactive = 1,

        /// <summary>
        /// 啟用中
        /// </summary>
        Active = 2,

        /// <summary>
        /// 已完成
        /// </summary>
        Completed = 3,

        /// <summary>
        /// 取消
        /// </summary>
        Cancel = 4,
    }
}