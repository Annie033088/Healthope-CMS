using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.GroupClassSchedule.Request
{
    public class RequestGetShowcaseAndCoachDto
    {
        /// <summary>
        /// 篩選分類
        /// </summary>
        public int? Category { get; set; }
    }
}