using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
