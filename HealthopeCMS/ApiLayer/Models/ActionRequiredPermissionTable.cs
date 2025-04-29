using System.Collections.Generic;
using DomainLayer.Models;

namespace ApiLayer.Models
{
    public class ActionRequiredPermissionTable
    {
        // 字典的 key：controller 跟 action  ; value：需要的權限 (List)
        public readonly Dictionary<string, List<AdminPermission>> actionRequiredPermission = new Dictionary<string, List<AdminPermission>>()
        {
            { "Admin,AddAdmin", new List<AdminPermission> { AdminPermission.EditAdmin} }
        };
    }
}