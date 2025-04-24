using DomainLayer.Models;

namespace PersistentLayer.Interface
{
    public interface IAdminRepository
    {
        /// <summary>
        /// 取得登入中的管理員資料
        /// </summary>
        Admin GetLoggingInAdmin(string account);

        /// <summary>
        /// 新增管理員
        /// </summary>
        bool AddAdmin(Admin admin);
    }
}
