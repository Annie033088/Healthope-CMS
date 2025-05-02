using System.Collections.Generic;
using DomainLayer.Models;
using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface IAdminRepository
    {
        /// <summary>
        /// 取得正要登入的管理員資料
        /// </summary>
        Admin GetLoggingInAdmin(string account);

        /// <summary>
        /// 修改自己的密碼
        /// </summary>
        bool EditSelfPwd(EditPwdDto editPwdDto);

        /// <summary>
        /// 新增管理員
        /// </summary>
        bool AddAdmin(Admin admin);

        /// <summary>
        /// 新增管理員
        /// </summary>
        (List<Admin> admins, int totalPage) GetAdmin(RequestGetAdminDto getAdminDto);

        /// <summary>
        /// 根據 Id 取得管理者
        /// </summary>
        Admin GetAdminById(int adminId);

        /// <summary>
        /// 修改管理者
        /// </summary>
        bool EditAdmin(RequestEditAdminDto editAdminDto);

        /// <summary>
        /// 刪除管理者
        /// </summary>
        bool DeleteAdmin(int adminId);
    }
}
