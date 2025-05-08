using System;
using System.Collections.Generic;

namespace ApiLayer.Models.Admin.ResponseAdminDto
{
    public class ResponseGetAdminListDto
    {
        /// <summary>
        /// 管理者清單
        /// </summary>
        public List<ResponseGetAdminDto> AdminList { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }
    }

    public class ResponseGetAdminDto
    {
        /// <summary>
        /// 管理員 Id
        /// </summary>
        public int AdminId { get; set; }

        /// <summary>
        /// 管理員帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 管理員狀態
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 身份(對應權限)
        /// </summary>
        public byte Identity { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}