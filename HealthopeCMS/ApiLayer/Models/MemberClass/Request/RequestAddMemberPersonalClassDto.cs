using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.MemberClass.Request
{
    public class RequestAddMemberPersonalClassDto
    {
        /// <summary>
        /// 會員的教練課方案 Id
        /// </summary>
        public int MemberPersonalTrainingPackageId { get; set; }

        /// <summary>
        /// 會員 Id
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 教練 Id
        /// </summary>
        public int CoachId { get; set; }

        /// <summary>
        /// 上課時間
        /// </summary>
        public DateTime Time { get; set; }
    }
}