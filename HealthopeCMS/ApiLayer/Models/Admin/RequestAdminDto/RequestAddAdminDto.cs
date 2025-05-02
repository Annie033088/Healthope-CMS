using System.ComponentModel.DataAnnotations;

namespace ApiLayer.Models.Admin.RequestAdminDto
{
    public class RequestAddAdminDto
    {
        // TODO: 把驗證移到 controller
        /// <summary>
        /// 管理員帳號
        /// </summary>
        [RegularExpression("^(?=.*[a-zA-Z])(?=.*\\d)[a-zA-Z\\d]{8,20}$", ErrorMessage = "請輸入8~20位英文數字")]
        public string Account { get; set; }

        /// <summary>
        /// 管理員未加密密碼
        /// </summary>
        [RegularExpression("^(?=.*[a-zA-Z])(?=.*\\d)[a-zA-Z\\d]{8,20}$", ErrorMessage = "請輸入8~20位英文數字")]
        public string Pwd { get; set; }

        /// <summary>
        /// 身份(對應權限)
        /// </summary>
        public string Identity { get; set; }
    }
}