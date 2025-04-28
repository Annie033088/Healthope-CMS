using System;
using System.Collections.Generic;
using DomainLayer.Models;

namespace DomainLayer.Utility
{
    public class AdminPermissionUtility
    {
        private readonly AdminPermissionDictionary adminPermissionDictionary;
        public AdminPermissionUtility()
        {
            adminPermissionDictionary = new AdminPermissionDictionary();
        }
        /// <summary>
        /// 判斷有無此權限
        /// </summary>
        public bool HasPermission(AdminIdentity identity, AdminPermission permission)
        {
            try
            {
                return adminPermissionDictionary.IdentityPermission.TryGetValue(identity, out List<AdminPermission> permissions) && permissions.Contains(permission);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得此身分的權限
        /// </summary>
        public List<AdminPermission> GetPermissions(AdminIdentity identity)
        {
            try
            {
                return adminPermissionDictionary.IdentityPermission.TryGetValue(identity, out List<AdminPermission> permissions) ? permissions : null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}