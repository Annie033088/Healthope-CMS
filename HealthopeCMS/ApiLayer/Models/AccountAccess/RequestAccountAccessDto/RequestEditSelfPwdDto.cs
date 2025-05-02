namespace ApiLayer.Models.AccountAccess.RequestAccountAccessDto
{
    public class RequestEditSelfPwdDto
    {
        /// <summary>
        /// 管理員舊密碼
        /// </summary>
        public string OldPwd { get; set; }

        /// <summary>
        /// 管理員新密碼
        /// </summary>
        public string NewPwd { get; set; }
    }
}