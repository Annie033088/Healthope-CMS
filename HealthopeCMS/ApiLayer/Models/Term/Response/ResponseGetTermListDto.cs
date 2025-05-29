using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.Term.Response
{
    public class ResponseGetTermListDto
    {
        /// <summary>
        /// 條款清單
        /// </summary>
        public List<ResponseGetTermDto> TermList {  get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }
    }
    public class ResponseGetTermDto
    {
        /// <summary>
        /// 條款 Id
        /// </summary>
        public int TermId { get; set; }

        /// <summary>
        /// 版本號
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 類型
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 適用對象
        /// </summary>
        public byte ApplicableTarget { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 條款生效日
        /// </summary>
        public DateTime EffectiveTime { get; set; }
    }
}