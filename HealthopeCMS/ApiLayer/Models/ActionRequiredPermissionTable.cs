using System.Collections.Generic;
using DomainLayer.Models;

namespace ApiLayer.Models
{
    public class ActionRequiredPermissionTable
    {
        // 字典的 key：controller 跟 action  ; value：需要的任一權限 (List)
        public readonly Dictionary<string, List<AdminPermission>> actionRequiredPermission = new Dictionary<string, List<AdminPermission>>()
        {
            // AdminController
            { "Admin,AddAdmin", new List<AdminPermission> {
                AdminPermission.EditAdmin} },
            { "Admin,GetAdmin", new List<AdminPermission> {
                AdminPermission.EditAdmin} },
            { "Admin,GetAdminById", new List<AdminPermission> {
                AdminPermission.EditAdmin} },
            { "Admin,EditAdmin", new List<AdminPermission> {
                AdminPermission.EditAdmin} },
            { "Admin,DeleteAdmin", new List<AdminPermission> {
                AdminPermission.EditAdmin} },

            // MemberController
            { "Member,GetMember", new List<AdminPermission> {
                AdminPermission.SelectMember, AdminPermission.EditMember } },
            { "Member,GetMemberEditDataById", new List<AdminPermission> {
                AdminPermission.EditMember} },
            { "Member,EditMember", new List<AdminPermission> {
                AdminPermission.EditMember} },
            { "Member,GetMemberDetail", new List<AdminPermission> {
                AdminPermission.SelectMember} },

            // CoachController
            { "Coach,GetCoach", new List<AdminPermission> {
                AdminPermission.AddCoach, AdminPermission.SelectCoach, AdminPermission.EditCoach} },
            { "Coach,AddCoach", new List<AdminPermission> {
                AdminPermission.AddCoach} },
            { "Coach,GetCoachEditDataById", new List<AdminPermission> {
                AdminPermission.EditCoach} },
            { "Coach,EditCoach", new List<AdminPermission> {
                AdminPermission.EditCoach} },

            // GroupClassShowcaseController
            { "GroupClassShowcase,AddShowcase", new List<AdminPermission> {
                AdminPermission.EditGroupClassShowcase} },
            { "GroupClassShowcase,GetShowcase", new List<AdminPermission> {
                AdminPermission.EditGroupClassShowcase, AdminPermission.SelectGroupClassShowcase} },
            { "GroupClassShowcase,GetShowcaseDetail", new List<AdminPermission> {
                AdminPermission.EditGroupClassShowcase, AdminPermission.SelectGroupClassShowcase} },
            { "GroupClassShowcase,GetShowcaseEditDataById", new List<AdminPermission> {
                AdminPermission.EditGroupClassShowcase,} },
            { "GroupClassShowcase,EditShowcase", new List<AdminPermission> {
                AdminPermission.EditGroupClassShowcase,} },
            { "GroupClassShowcase,DeleteShowcase", new List<AdminPermission> {
                AdminPermission.EditGroupClassShowcase,} },

            // GroupClassScheduleController
            { "GroupClassSchedule,GetShowcaseAndCoach", new List<AdminPermission> {
                AdminPermission.EditGroupClassSchedule,} },
            { "GroupClassSchedule,AddSchedule", new List<AdminPermission> {
                AdminPermission.EditGroupClassSchedule,} },
            { "GroupClassSchedule,GetSchedule", new List<AdminPermission> {
                AdminPermission.EditGroupClassSchedule, AdminPermission.SelectGroupClassSchedule} },

            // PlanTemplateController
            { "PlanTemplate,AddTicketPlan", new List<AdminPermission> {
                AdminPermission.EditPlan,} },
            { "PlanTemplate,AddMembershipPlan", new List<AdminPermission> {
                AdminPermission.EditPlan,} },
            { "PlanTemplate,AddPersonalTrainingPackage", new List<AdminPermission> {
                AdminPermission.EditPlan,} },
            { "PlanTemplate,EditTicketPlanStatus", new List<AdminPermission> {
                AdminPermission.EditPlan,} },
            { "PlanTemplate,GetMembershipPlanEditDataById", new List<AdminPermission> {
                AdminPermission.EditPlan,} },
            { "PlanTemplate,GetPersonalTrainingPackageEditDataById", new List<AdminPermission> {
                AdminPermission.EditPlan,} },
            { "PlanTemplate,EditMembershipPlan", new List<AdminPermission> {
                AdminPermission.EditPlan,} },
            { "PlanTemplate,EditPersonalTrainingPackage", new List<AdminPermission> {
                AdminPermission.EditPlan,} },
            { "PlanTemplate,GetMembershipPlan", new List<AdminPermission> {
                AdminPermission.EditPlan, AdminPermission.SelectPlan} },
            { "PlanTemplate,GetPersionalTrainingPackage", new List<AdminPermission> {
                AdminPermission.EditPlan, AdminPermission.SelectPlan} },
            { "PlanTemplate,GetTicketPlan", new List<AdminPermission> {
                AdminPermission.EditPlan, AdminPermission.SelectPlan} },

            // TermController
            { "Term,GetOldTerm", new List<AdminPermission> {
                AdminPermission.EditTerm, AdminPermission.SelectTerm} },
            { "Term,GetTerm", new List<AdminPermission> {
                AdminPermission.EditTerm, AdminPermission.SelectTerm} },
            { "Term,AddTerm", new List<AdminPermission> {
                AdminPermission.EditTerm} },
        };
    }
}