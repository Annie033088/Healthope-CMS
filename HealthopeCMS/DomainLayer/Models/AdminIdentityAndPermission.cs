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
        /// 查詢會員權限
        /// </summary>
        SelectMember,

        /// <summary>
        /// 修改會員權限
        /// </summary>
        EditMember,

        /// <summary>
        /// 查詢教練權限
        /// </summary>
        SelectCoach,

        /// <summary>
        /// 新增教練權限
        /// </summary>
        AddCoach,

        /// <summary>
        /// 修改教練權限
        /// </summary>
        EditCoach,
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
        public readonly Dictionary<AdminIdentity, List<AdminPermission>> IdentityPermission
            = new Dictionary<AdminIdentity, List<AdminPermission>>()
        {
            { AdminIdentity.SuperAdmin, new List<AdminPermission> {
                AdminPermission.EditAdmin, AdminPermission.EditMember,
                AdminPermission.SelectMember, AdminPermission.EditMember,
                AdminPermission.SelectCoach, AdminPermission.AddCoach, AdminPermission.EditCoach,
            } },
            { AdminIdentity.Admin, new List<AdminPermission> {
                AdminPermission.SelectMember, AdminPermission.EditMember,
                AdminPermission.SelectCoach, AdminPermission.AddCoach, AdminPermission.EditCoach,
            } },
            {AdminIdentity.Receptionist, new List<AdminPermission>{
                AdminPermission.SelectMember,
            } },
            {AdminIdentity.CoachManager, new List<AdminPermission>{
                AdminPermission.SelectMember,
                AdminPermission.SelectCoach, AdminPermission.AddCoach, AdminPermission.EditCoach,
            } },
            {AdminIdentity.SalesRepresentative, new List<AdminPermission>{
                AdminPermission.SelectMember
            } }
        };
    }
}