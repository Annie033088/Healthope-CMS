using System.Collections.Generic;
using ApiLayer.Models.Admin.RequestAdminDto;
using DomainLayer.Models;

namespace ApiLayer.Interface
{
    public interface IAdminService
    {
        /// <summary>
        /// 新增管理者(帳密)
        /// </summary>
        bool AddAdmin(RequestAddAdminDto addAdminDto);

        /// <summary>
        /// 取得當前登入帳號權限
        /// </summary>
        List<AdminPermission> GetPermission();
    }
}
