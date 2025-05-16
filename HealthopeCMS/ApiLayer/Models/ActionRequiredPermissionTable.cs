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
            { "Coach,GetMember", new List<AdminPermission> { AdminPermission.SelectMember, AdminPermission.EditMember } },
            { "Coach,GetMemberEditDataById", new List<AdminPermission> { AdminPermission.EditMember} },
            { "Coach,EditMember", new List<AdminPermission> { AdminPermission.EditMember} },
            { "Coach,GetMemberDetail", new List<AdminPermission> { AdminPermission.SelectMember} },
            { "Coach,AddCoach", new List<AdminPermission> { AdminPermission.AddCoach} },
        };
    }
}