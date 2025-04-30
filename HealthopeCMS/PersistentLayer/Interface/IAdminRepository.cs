using System.Collections.Generic;
using ApiLayer.Models.Admin.RequestAdminDto;
using DomainLayer.Models;

namespace PersistentLayer.Interface
{
    public interface IAdminRepository
    {
        /// <summary>
        /// 取得正要登入的管理員資料
        /// </summary>
        Admin GetLoggingInAdmin(string account);

        /// <summary>
        /// 新增管理員
        /// </summary>
        bool AddAdmin(Admin admin);

        /// <summary>
        /// 新增管理員
        /// </summary>
        (List<Admin> admins, int totalPage) GetAdmin(RequestGetAdminDto getAdminDto);

        /// <summary>
        /// 修改管理者
        /// </summary>
        bool EditAdmin(RequestEditAdminDto editAdminDto);

        /// <summary>
        /// 根據 Id 取得管理者
        /// </summary>
        Admin GetAdminById(int adminId);
    }
}
