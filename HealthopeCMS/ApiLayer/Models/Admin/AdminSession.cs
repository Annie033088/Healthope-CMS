using System;
using DomainLayer.Models;

namespace ApiLayer.Models.Admin
{
    [Serializable]
    public class AdminSession
    {
        /// <summary>
        /// 管理員 Id
        /// </summary>
        public int AdminId { get; set; }

        /// <summary>
        /// 身份(對應權限)
        /// </summary>
        public AdminIdentity Identity { get; set; }
    }
}