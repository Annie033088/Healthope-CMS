namespace ApiLayer.Models.Member.Response
{
    public class ResponseGetMemberByNameOrPhoneDto
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
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public bool Status { get; set; }
    }
}