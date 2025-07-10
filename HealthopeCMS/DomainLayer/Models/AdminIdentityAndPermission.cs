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
        EditAdmin = 1,

        /// <summary>
        /// 查詢會員權限
        /// </summary>
        SelectMember = 2,

        /// <summary>
        /// 修改會員權限
        /// </summary>
        EditMember = 3,

        /// <summary>
        /// 查詢教練權限
        /// </summary>
        SelectCoach = 4,

        /// <summary>
        /// 新增教練權限
        /// </summary>
        AddCoach = 5,

        /// <summary>
        /// 修改教練權限
        /// </summary>
        EditCoach = 6,

        /// <summary>
        /// 增刪修 展示團課 權限
        /// </summary>
        EditGroupClassShowcase = 7,

        /// <summary>
        /// 查詢 展示團課 權限
        /// </summary>
        SelectGroupClassShowcase = 8,

        /// <summary>
        /// 增刪修 團課表 權限
        /// </summary>
        EditGroupClassSchedule = 9,

        /// <summary>
        /// 查詢 團課表 權限
        /// </summary>
        SelectGroupClassSchedule = 10,

        /// <summary>
        /// 增刪修 團課表 權限
        /// </summary>
        EditPlan = 11,

        /// <summary>
        /// 查詢 團課表 權限
        /// </summary>
        SelectPlan = 12,

        /// <summary>
        /// 增修 會員預約課程
        /// </summary>
        EditMemberClass = 13,

        /// <summary>
        /// 查詢 會員預約課程
        /// </summary>
        SelectMemberClass = 14,

        /// <summary>
        /// 修改條款
        /// </summary>
        EditTerm = 15,

        /// <summary>
        /// 查詢條款
        /// </summary>
        SelectTerm = 16,

        /// <summary>
        /// 修改租約
        /// </summary>
        EditLeaseAgreement = 17,

        /// <summary>
        /// 查詢租約
        /// </summary>
        SelectLeaseAgreement = 18,

        /// <summary>
        /// 建立訂單(銷售員)
        /// </summary>
        AddOrder = 19,

        /// <summary>
        /// 修改訂單 (EX:退款)
        /// </summary>
        EditOrder = 20,

        /// <summary>
        /// 查詢訂單
        /// </summary>
        SelectOrder = 21,

        /// <summary>
        /// 查詢付款紀錄
        /// </summary>
        SelectTransaction = 22,

        /// <summary>
        /// 暫停會籍
        /// </summary>
        EditMemberMembershipPlan = 23,

        /// <summary>
        /// 修改會員的私人課程教練 
        /// </summary>
        EditMemberPersonalPeckagePlan = 24,

        /// <summary>
        /// 查看財務報表
        /// </summary>
        SelectFinancialStatements = 25,

        /// <summary>
        /// 查看教練課報表
        /// </summary>
        SelectCoachReport = 26,
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
                AdminPermission.EditAdmin,
                AdminPermission.SelectMember, AdminPermission.EditMember,
                AdminPermission.SelectCoach, AdminPermission.AddCoach, AdminPermission.EditCoach,
                AdminPermission.EditGroupClassShowcase, AdminPermission.SelectGroupClassShowcase,
                AdminPermission.EditGroupClassSchedule, AdminPermission.SelectGroupClassSchedule,
                AdminPermission.EditPlan, AdminPermission.SelectPlan,
                AdminPermission.EditTerm, AdminPermission.SelectTerm,
                AdminPermission.EditLeaseAgreement, AdminPermission.SelectLeaseAgreement,
                AdminPermission.AddOrder, AdminPermission.EditOrder, AdminPermission.SelectOrder,
                AdminPermission.SelectTransaction,
                AdminPermission.EditMemberMembershipPlan,
                AdminPermission.EditMemberPersonalPeckagePlan,
                AdminPermission.SelectFinancialStatements,
                AdminPermission.SelectCoachReport,
                AdminPermission.EditMemberClass, AdminPermission.SelectMemberClass,
            } },
            { AdminIdentity.Admin, new List<AdminPermission> {
                AdminPermission.SelectMember, AdminPermission.EditMember,
                AdminPermission.SelectCoach, AdminPermission.AddCoach, AdminPermission.EditCoach,
                AdminPermission.EditGroupClassShowcase, AdminPermission.SelectGroupClassShowcase,
                AdminPermission.EditGroupClassSchedule, AdminPermission.SelectGroupClassSchedule,
                AdminPermission.EditPlan, AdminPermission.SelectPlan,
                AdminPermission.EditTerm, AdminPermission.SelectTerm,
                AdminPermission.EditLeaseAgreement, AdminPermission.SelectLeaseAgreement,
                AdminPermission.AddOrder, AdminPermission.EditOrder, AdminPermission.SelectOrder,
                AdminPermission.SelectTransaction,
                AdminPermission.EditMemberMembershipPlan,
                AdminPermission.EditMemberPersonalPeckagePlan,
                AdminPermission.SelectFinancialStatements,
                AdminPermission.SelectCoachReport,
                AdminPermission.EditMemberClass, AdminPermission.SelectMemberClass,
            } },
            {AdminIdentity.Receptionist, new List<AdminPermission>{
                AdminPermission.SelectMember,
                AdminPermission.SelectGroupClassShowcase,
                AdminPermission.SelectGroupClassSchedule,
                AdminPermission.AddOrder,
                AdminPermission.EditMemberMembershipPlan,
            } },
            {AdminIdentity.CoachManager, new List<AdminPermission>{
                AdminPermission.SelectMember,
                AdminPermission.SelectCoach, AdminPermission.AddCoach, AdminPermission.EditCoach,
                AdminPermission.EditMemberPersonalPeckagePlan,
                AdminPermission.SelectCoachReport,
                AdminPermission.EditMemberClass, AdminPermission.SelectMemberClass,
            } },
            {AdminIdentity.CourseManager, new List<AdminPermission>{
                AdminPermission.EditGroupClassShowcase,AdminPermission.SelectGroupClassShowcase,
                AdminPermission.EditGroupClassSchedule, AdminPermission.SelectGroupClassSchedule,
            } },
            {AdminIdentity.SalesRepresentative, new List<AdminPermission>{
                AdminPermission.SelectMember,
            } },
            {AdminIdentity.Accountant, new List<AdminPermission>{
                AdminPermission.SelectTransaction,
                AdminPermission.SelectFinancialStatements,
                AdminPermission.SelectCoachReport,
            } }
        };
    }
}