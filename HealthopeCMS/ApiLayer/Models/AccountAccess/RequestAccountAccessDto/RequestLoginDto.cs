namespace ApiLayer.Models.AccountAccess.RequestAccountAccessDto
{
    public class RequestLoginDto
    {
        /// <summary>
        /// 管理員帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 管理員密碼
        /// </summary>
        public string Pwd { get; set; }
    }
}