using System.Collections.Generic;
using ApiLayer.Models;
using ApiLayer.Models.AccountAccess.RequestAccountAccessDto;
using DomainLayer.Models;

namespace ApiLayer.Interface
{
    public interface IAccountAccessService
    {
        /// <summary>
        /// 管理員登入
        /// </summary>
        (bool success, Admin admin) AdminLogin(RequestLoginDto loginDto);

        /// <summary>
        /// 管理者登出
        /// </summary>
        bool AdminLogout();

        /// <summary>
        /// 管理者是否已登入
        /// </summary>
        ErrorCodeDefine AdminHavePermission(List<RequestPermissionDto> havePermissionDto);

        /// <summary>
        /// 管理者修改自己的密碼
        /// </summary>
        ErrorCodeDefine EditSelfPwd(RequestEditSelfPwdDto editSelfPwdDto);
    }
}
