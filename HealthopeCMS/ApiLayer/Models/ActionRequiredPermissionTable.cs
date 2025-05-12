using System.Collections.Generic;
using DomainLayer.Models;

namespace ApiLayer.Models
{
    public class ActionRequiredPermissionTable
    {
        // 字典的 key：controller 跟 action  ; value：需要的任一權限 (List)
        public readonly Dictionary<string, List<AdminPermission>> actionRequiredPermission = new Dictionary<string, List<AdminPermission>>()
        {
            { "Admin,AddAdmin", new List<AdminPermission> { AdminPermission.EditAdmin} },
            { "Admin,GetAdmin", new List<AdminPermission> { AdminPermission.EditAdmin} },
            { "Admin,GetAdminById", new List<AdminPermission> { AdminPermission.EditAdmin} },
            { "Admin,EditAdmin", new List<AdminPermission> { AdminPermission.EditAdmin} },
            { "Admin,DeleteAdmin", new List<AdminPermission> { AdminPermission.EditAdmin} },
            { "Member,GetMember", new List<AdminPermission> { AdminPermission.SelectMember, AdminPermission.EditMember } },
            { "Member,GetMemberEditDataById", new List<AdminPermission> { AdminPermission.EditMember} },
            { "Member,EditMember", new List<AdminPermission> { AdminPermission.EditMember} },
        };
    }
}