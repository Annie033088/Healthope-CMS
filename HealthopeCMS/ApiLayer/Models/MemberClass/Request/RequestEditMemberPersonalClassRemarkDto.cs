using System;

namespace ApiLayer.Models.MemberClass.Request
{
    public class RequestEditMemberPersonalClassRemarkDto
    {
        /// <summary>
        /// 會員預約的私人課 Id
        /// </summary>
        public int MemberPersonalClassId { get; set; }

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