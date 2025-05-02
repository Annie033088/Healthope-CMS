using System.Collections.Generic;
using ApiLayer.Models.Admin.RequestAdminDto;
using ApiLayer.Models.Admin.ResponseAdminDto;
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

        /// <summary>
        /// 取得管理者列表
        /// </summary>
        ResponseGetAdminListDto GetAdmin(RequestGetAdminDto getAdminDto);

        /// <summary>
        /// 修改管理者
        /// </summary>
        bool EditAdmin(RequestEditAdminDto editAdminDto);

        /// <summary>
        /// 根據 Id 取得管理者
        /// </summary>
        ResponseGetAdminDto GetAdminById(RequestAdminIdDto getAdminDto);

        /// <summary>
        /// 刪除管理者
        /// </summary>
        bool DeleteAdmin(RequestAdminIdDto adminIdDto);
    }
}
