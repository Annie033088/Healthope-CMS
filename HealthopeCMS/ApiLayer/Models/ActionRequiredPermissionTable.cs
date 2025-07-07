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
                AdminPermission.SelectMember, AdminPermission.EditMember} },
            { "Member,GetMemberByNameOrPhone", new List<AdminPermission> {
                AdminPermission.SelectMember, AdminPermission.EditMember} },

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
            { "PlanTemplate,GetAllTypePlan", new List<AdminPermission> {
                AdminPermission.EditPlan, AdminPermission.SelectPlan,
                AdminPermission.AddOrder, AdminPermission.EditOrder} },

            // TermController
            { "Term,GetOldTerm", new List<AdminPermission> {
                AdminPermission.EditTerm, AdminPermission.SelectTerm} },
            { "Term,GetTerm", new List<AdminPermission> {
                AdminPermission.EditTerm, AdminPermission.SelectTerm} },
            { "Term,AddTerm", new List<AdminPermission> {
                AdminPermission.EditTerm} },
            { "Term,GetTermEditDataById", new List<AdminPermission> {
                AdminPermission.EditTerm} },
            { "Term,EditTerm", new List<AdminPermission> {
                AdminPermission.EditTerm} },
            { "Term,EditTermStatus", new List<AdminPermission> {
                AdminPermission.EditTerm} },
            { "Term,GetTermDetail", new List<AdminPermission> {
                AdminPermission.EditTerm, AdminPermission.SelectTerm} },
            { "Term,DeleteTerm", new List<AdminPermission> {
                AdminPermission.EditTerm} },

            // LeaseAgreementController
            { "LeaseAgreement,AddLeaseAgreement", new List<AdminPermission> {
                AdminPermission.EditLeaseAgreement} },
            { "LeaseAgreement,GetLeaseAgreement", new List<AdminPermission> {
                AdminPermission.EditLeaseAgreement, AdminPermission.SelectLeaseAgreement} },
            { "LeaseAgreement,EditLeaseAgreementStatus", new List<AdminPermission> {
                AdminPermission.EditLeaseAgreement} },
            { "LeaseAgreement,EditLeaseAgreementRemind", new List<AdminPermission> {
                AdminPermission.EditLeaseAgreement} },
            { "LeaseAgreement,DeleteLeaseAgreement", new List<AdminPermission> {
                AdminPermission.EditLeaseAgreement} },

            // InvoiceController
            { "Invoice,AddInvoiceTrackNumber", new List<AdminPermission> {
                AdminPermission.EditOrder} },
            { "Invoice,GetInvoiceTrackNumber", new List<AdminPermission> {
                AdminPermission.EditOrder, AdminPermission.SelectOrder} },
            { "Invoice,EditInvoiceTrackNumberStatus", new List<AdminPermission> {
                AdminPermission.EditOrder} },
            { "Invoice,DeleteInvoiceTrackNumber", new List<AdminPermission> {
                AdminPermission.EditOrder} },
            { "Invoice,CompleteOrderAndPrintInvoice", new List<AdminPermission> {
                AdminPermission.AddOrder, AdminPermission.EditOrder} },
            { "Invoice,VoidInvoice", new List<AdminPermission> {
                AdminPermission.EditOrder} },
            { "Invoice,DiscountInvoice", new List<AdminPermission> {
                AdminPermission.EditOrder} },
            { "Invoice,PendingVoidInvoice", new List<AdminPermission> {
                AdminPermission.EditOrder} },
            { "Invoice,PendingDiscountInvoice", new List<AdminPermission> {
                AdminPermission.EditOrder} },

            // OrderController
            { "Order,AddOrder", new List<AdminPermission> {
                AdminPermission.AddOrder, AdminPermission.EditOrder} },
            { "Order,PayByCash", new List<AdminPermission> {
                AdminPermission.AddOrder, AdminPermission.EditOrder} },
            { "Order,PayByCard", new List<AdminPermission> {
                AdminPermission.AddOrder, AdminPermission.EditOrder} },
            { "Order,GetOrder", new List<AdminPermission> {
                AdminPermission.AddOrder, AdminPermission.EditOrder, AdminPermission.SelectOrder} },
            { "Order,GetOrderDetailById", new List<AdminPermission> {
                AdminPermission.AddOrder, AdminPermission.EditOrder, AdminPermission.SelectOrder} },
            { "Order,EditOrderStateRemark", new List<AdminPermission> {
                AdminPermission.AddOrder, AdminPermission.EditOrder} },
            { "Order,EditOrderRemark", new List<AdminPermission> {
                AdminPermission.AddOrder, AdminPermission.EditOrder} },
            { "Order,CancelPendingOrder", new List<AdminPermission> {
                AdminPermission.AddOrder, AdminPermission.EditOrder} },
            { "Order,RefundIn7Days", new List<AdminPermission> {
                AdminPermission.EditOrder} },
            { "Order,CheckoutRefundQualifyAndTerminateOrder", new List<AdminPermission> {
                AdminPermission.EditOrder} },
            { "Order,TerminateOrder", new List<AdminPermission> {
                AdminPermission.EditOrder} },
            { "Order,CheckoutRefundQualifyAndBreachOrder", new List<AdminPermission> {
                AdminPermission.EditOrder} },
            { "Order,BreachOrder", new List<AdminPermission> {
                AdminPermission.EditOrder} },

            // TransactionController
            { "Transaction,GetTransaction", new List<AdminPermission> {
                AdminPermission.SelectTransaction} },
            { "Transaction,GetCreditCardCashFlowData", new List<AdminPermission> {
                AdminPermission.SelectTransaction} },

            // RefundController
            { "Refund,GetRefund", new List<AdminPermission> {
                AdminPermission.SelectTransaction} },
        };
    }
}