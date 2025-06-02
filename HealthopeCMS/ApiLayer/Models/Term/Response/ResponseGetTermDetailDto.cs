using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.Term.Response
{
    public class ResponseGetTermDetailDto
    {
        /// <summary>
        /// 版本號
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述更新內容
        /// </summary>
        public string VersionDescription { get; set; }

        /// <summary>
        /// 內文
        /// </summary>
        public string DetailContent { get; set; }
    }
}