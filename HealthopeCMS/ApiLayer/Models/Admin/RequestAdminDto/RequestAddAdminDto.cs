namespace ApiLayer.Models.Admin.RequestAdminDto
{
    public class RequestAddAdminDto
    {
        /// <summary>
        /// 管理員帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 管理員未加密密碼
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 身份(對應權限)
        /// </summary>
        public string Identity { get; set; }
    }
}