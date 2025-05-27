using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Models.Other;
using ApiLayer.Models.PlanTemplate.Request;
using ApiLayer.Models.PlanTemplate.Response;
using ApiLayer.Models.Response.PlanTemplate;
using DomainLayer.Models;

namespace ApiLayer.Interface
{
    public interface IPlanTemplateService
    {
        /// <summary>
        /// 新增 一次性票劵方案
        /// </summary>
        bool AddTicketPlan(RequestAddTicketPlanDto addTicketPlanDto);

        /// <summary>
        /// 新增 會籍方案
        /// </summary>
        (bool successFlag, Exception exception) AddMembershipPlan(
            RequestAddMembershipPlanDto addMembershipPlanDto, FileDto file);

        /// <summary>
        /// 新增 教練課方案
        /// </summary>
        (bool successFlag, Exception exception) AddPersonalTrainingPackage(
            RequestAddPersonalTrainingPackageDto addPersonalTrainingPackageDto, FileDto file);

        /// <summary>
        /// 取得會籍方案
        /// </summary>
        ResponseGetMembershipPlanListDto GetMembershipPlan(RequestGetPlanDto getPlanDto);

        /// <summary>
        /// 取得教練課方案
        /// </summary>
        ResponseGetPersonalTrainingPackageListDto GetPersionalTrainingPackage(RequestGetPlanDto getPlanDto);

        /// <summary>
        /// 取得票劵方案
        /// </summary>
        ResponseGetTicketPlanListDto GetTicketPlan(RequestGetPlanDto getPlanDto);
    }
}
