using System;
using System.Web.Http;
using ApiLayer.Models;
using ApiLayer.Models.Other;
using ApiLayer.Models.PlanTemplate.Request;
using ApiLayer.Models.PlanTemplate.Response;
using ApiLayer.Models.PlanTemplate.Response.GetAllType;
using ApiLayer.Models.Response.PlanTemplate;
using PersistentLayer.Models;

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

        /// <summary>
        /// 修改票劵方案狀態
        /// </summary>
        bool EditTicketPlanStatus(RequestEditStatusDto editStatusDto);

        /// <summary>
        /// 取得修改會籍方案頁面資料
        /// </summary>
        ResponseGetMembershipPlanEditDataDto GetMembershipPlanEditDataById(RequestMembershipPlanIdDto memebershipPlanIdDto);

        /// <summary>
        /// 取得修改教練課方案頁面資料
        /// </summary>
        ResponseGetPersonalTrainingPackageEditDataDto GetPersonalTrainingPackageEditDataById(
            RequestPersonalTrainingPackageIdDto personalTrainingPackageIdDto);

        /// <summary>
        /// 修改會籍方案
        /// </summary>
        (ErrorCodeDefine errorCode, Exception exception) EditMembershipPlan(
            RequestEditMembershipPlanDto editMembershipPlanDto, FileDto file);

        /// <summary>
        /// 修改教練課方案
        /// </summary>
        (ErrorCodeDefine errorCode, Exception exception) EditPersonalTrainingPackage(
            RequestEditPersonalTrainingPackageDto editPlanDto, FileDto file);

        /// <summary>
        /// (新增訂單時) 取得所有方案
        /// </summary>
        ResponseGetAllTypePlanDto GetAllTypePlan();
    }
}
