using System.Collections.Generic;
using ApiLayer.Models.PlanTemplate.Request;
using DomainLayer.Models;
using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface IPlanTemplateRepository
    {
        /// <summary>
        /// 新增 一次性票劵方案
        /// </summary>
        bool AddTicketPlan(TicketPlan ticketPlan);

        /// <summary>
        /// 新增 會籍方案
        /// </summary>
        ResultWithException AddMembershipPlan(MembershipPlan membershipPlan);

        /// <summary>
        /// 新增 教練課方案
        /// </summary>
        ResultWithException AddPersonalTrainingPackage(PersonalTrainingPackage personalTrainingPackage);

        /// <summary>
        /// 取得會籍方案
        /// </summary>
        (List<MembershipPlan> membershipPlans, int totalPage) GetMembershipPlan(RequestGetPlanDto getPlanDto);

        /// <summary>
        /// 取得教練課方案
        /// </summary>
        (List<PersonalTrainingPackage> personalTrainingPackages, int totalpage)
            GetPersionalTrainingPackage(RequestGetPlanDto getPlanDto);

        /// <summary>
        /// 取得票劵方案
        /// </summary>
        (List<TicketPlan> ticketPlans, int totalPage) GetTicketPlan(RequestGetPlanDto getPlanDto);

        /// <summary>
        /// 修改票劵方案狀態
        /// </summary>
        bool EditTicketPlanStatus(TicketPlan ticketPlan);

        /// <summary>
        /// 取得修改會籍方案頁面資料
        /// </summary>
        MembershipPlan GetMembershipPlanEditDataById(int memebershipPlanId);

        /// <summary>
        /// 取得修改教練課方案頁面資料
        /// </summary>
        PersonalTrainingPackage GetPersonalTrainingPackageEditDataById(int personalTrainingPackageId);

        /// <summary>
        /// 修改會籍方案
        /// </summary>
        (ResultWithException result, string oldImageUrl) EditMembershipPlan(
            RequestEditMembershipPlanDto editMembershipPlanDto);

        /// <summary>
        /// 修改教練課方案
        /// </summary>
        (ResultWithException result, string oldImageUrl) EditPersonalTrainingPackage(
            RequestEditPersonalTrainingPackageDto editPlanDto);

        /// <summary>
        /// (新增訂單時) 取得所有方案
        /// </summary>
        (List<MembershipPlan> membershipPlans, List<PersonalTrainingPackage> personalTrainingPackages,
            List<TicketPlan> ticketPlans) GetAllTypePlan();
    }
}
