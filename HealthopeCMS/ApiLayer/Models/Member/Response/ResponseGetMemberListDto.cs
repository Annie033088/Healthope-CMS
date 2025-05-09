using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.Member
{
    public class ResponseGetMemberListDto
    {
        /// <summary>
        /// 會員清單
        /// </summary>
        public List<ResponseGetMemberDto> MemberList { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }
    }
    public class ResponseGetMemberDto
    {
        /// <summary>
        /// 會員 Id
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 手機
        /// </summary>
        public int Phone { get; set; }

        /// <summary>
        /// 手機 OTP 是否驗證
        /// </summary>
        public bool PhoneVerified { get; set; }

        /// <summary>
        /// 信箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 今年團課缺席次數
        /// </summary>
        public short AbsenceTime { get; set; }

        /// <summary>
        /// 允許預約團課的時間(在那之前被禁止了)
        /// </summary>
        public DateTime AllowGroupClass { get; set; }

        /// <summary>
        /// 會籍到期日
        /// </summary>
        public DateTime MembershipExpiry { get; set; }
    }
}