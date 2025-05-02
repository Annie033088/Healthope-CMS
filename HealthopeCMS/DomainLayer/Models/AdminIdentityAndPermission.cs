using System.Collections.Generic;

namespace DomainLayer.Models
{
    public enum AdminPermission
    {
        /// <summary>
        /// 無
        /// </summary>
        None,

        /// <summary>
        /// 管理者相關權限
        /// </summary>
        EditAdmin,

        /// <summary>
        /// 會員相關權限
        /// </summary>
        EditMember,
    }

    public enum AdminIdentity : byte
    {
        /// <summary>
        /// 無
        /// </summary>
        None,

        /// <summary>
        /// SA
        /// </summary>
        SuperAdmin,

        /// <summary>
        /// 一般管理員
        /// </summary>
        Admin,

        /// <summary>
        /// 接待員 (櫃檯人員)
        /// </summary>
        Receptionist,

        /// <summary>
        /// 會計
        /// </summary>
        Accountant,

        /// <summary>
        /// 課程管理員
        /// </summary>
        CourseManager,

        /// <summary>
        /// 教練管理員
        /// </summary>
        CoachManager,

        /// <summary>
        /// 業務
        /// </summary>
        SalesRepresentative
    }

    public class AdminPermissionDictionary
    {
        /// <summary>
        /// 字典 身份對照權限
        /// </summary>
        public readonly Dictionary<AdminIdentity, List<AdminPermission>> IdentityPermission = new Dictionary<AdminIdentity, List<AdminPermission>>()
        {
            { AdminIdentity.SuperAdmin, new List<AdminPermission> { AdminPermission.EditAdmin, AdminPermission.EditMember } },
            { AdminIdentity.Admin, new List<AdminPermission> { AdminPermission.EditMember } },
        };
    }
}