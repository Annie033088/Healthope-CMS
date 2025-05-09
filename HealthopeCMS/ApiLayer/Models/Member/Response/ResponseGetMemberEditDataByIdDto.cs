using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.Member.Response
{
    public class ResponseGetMemberEditDataByIdDto
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 手機
        /// </summary>
        public int Phone { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}