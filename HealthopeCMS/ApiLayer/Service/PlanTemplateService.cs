using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Other;
using ApiLayer.Models.PlanTemplate.Request;
using ApiLayer.Models.PlanTemplate.Response;
using ApiLayer.Models.Response.PlanTemplate;
using AutoMapper;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;
using PersistentLayer.Repository;

namespace ApiLayer.Service
{
    public class PlanTemplateService : IPlanTemplateService
    {
        private readonly IPlanTemplateRepository planTemplateRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;
        private readonly IHttpService httpService;

        public PlanTemplateService(IPlanTemplateRepository planTemplateRepository, IMapper mapper,
            IFileService fileService, IHttpService httpService)
        {
            this.planTemplateRepository = planTemplateRepository;
            this.mapper = mapper;
            this.fileService = fileService;
            this.httpService = httpService;
        }

        /// <summary>
        /// 新增 一次性票劵方案
        /// </summary>
        public bool AddTicketPlan(RequestAddTicketPlanDto addTicketPlanDto)
        {
            TicketPlan ticketPlan = mapper.Map<TicketPlan>(addTicketPlanDto);
            return planTemplateRepository.AddTicketPlan(ticketPlan);
        }

        /// <summary>
        /// 新增 會籍方案
        /// </summary>
        public (bool successFlag, Exception exception) AddMembershipPlan(
            RequestAddMembershipPlanDto addMembershipPlanDto, FileDto file)
        {
            try
            {
                MembershipPlan membershipPlan = mapper.Map<MembershipPlan>(addMembershipPlanDto);
                membershipPlan.ImageUrl = string.Empty;

                string savePath = "";

                if (file != null)
                {
                    string imageExtension = fileService.GetImageExtension(file.MimeType);
                    string fileName = Guid.NewGuid().ToString() + imageExtension;
                    string folderPath = Path.Combine(httpService.GetRootPath(),
                        "assets", "images", "personalTrainingPackage");
                    savePath = Path.Combine(folderPath, fileName);
                    membershipPlan.ImageUrl = Path.Combine("assets", "images", "PlanTemplate", fileName)
                        .Replace(Path.DirectorySeparatorChar, '/');
                    fileService.SaveFile(folderPath, savePath, file.FileData);
                }

                ResultWithException result = planTemplateRepository.AddMembershipPlan(membershipPlan);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), result.ErrorCodeNumber) || result.Exception != null)
                {
                    fileService.DeleteFile(savePath);
                    return (false, result.Exception);
                }

                ErrorCodeDefine errorCode = (ErrorCodeDefine)result.ErrorCodeNumber;

                if (errorCode != ErrorCodeDefine.Success)
                {
                    fileService.DeleteFile(savePath);
                    return (false, result.Exception);
                }

                return (true, result.Exception);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 新增 教練課方案
        /// </summary>
        public (bool successFlag, Exception exception) AddPersonalTrainingPackage(
            RequestAddPersonalTrainingPackageDto addPersonalTrainingPackageDto, FileDto file)
        {
            try
            {
                PersonalTrainingPackage personalTrainingPackage = mapper.Map<PersonalTrainingPackage>(addPersonalTrainingPackageDto);
                personalTrainingPackage.ImageUrl = string.Empty;

                string savePath = "";

                if (file != null)
                {
                    string imageExtension = fileService.GetImageExtension(file.MimeType);
                    string fileName = Guid.NewGuid().ToString() + imageExtension;
                    string folderPath = Path.Combine(httpService.GetRootPath(),
                        "assets", "images", "personalTrainingPackage");
                    savePath = Path.Combine(folderPath, fileName);
                    personalTrainingPackage.ImageUrl = Path.Combine("assets", "images", "PlanTemplate", fileName)
                        .Replace(Path.DirectorySeparatorChar, '/');
                    fileService.SaveFile(folderPath, savePath, file.FileData);
                }

                ResultWithException result = planTemplateRepository.AddPersonalTrainingPackage(personalTrainingPackage);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), result.ErrorCodeNumber) || result.Exception != null)
                {
                    fileService.DeleteFile(savePath);
                    return (false, result.Exception);
                }

                ErrorCodeDefine errorCode = (ErrorCodeDefine)result.ErrorCodeNumber;

                if (errorCode != ErrorCodeDefine.Success)
                {
                    fileService.DeleteFile(savePath);
                    return (false, result.Exception);
                }

                return (true, result.Exception);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得會籍方案
        /// </summary>
        public ResponseGetMembershipPlanListDto GetMembershipPlan(RequestGetPlanDto getPlanDto)
        {
            (List<MembershipPlan> membershipPlans, int totalPage) = planTemplateRepository.GetMembershipPlan(getPlanDto);
            ResponseGetMembershipPlanListDto response = new ResponseGetMembershipPlanListDto()
            {
                MembershipPlanList = mapper.Map<List<ResponseGetMembershipPlanDto>>(membershipPlans),
                TotalPage = totalPage,
            };
            return response;
        }

        /// <summary>
        /// 取得教練課方案
        /// </summary>
        public ResponseGetPersonalTrainingPackageListDto GetPersionalTrainingPackage(RequestGetPlanDto getPlanDto)
        {
            (List<PersonalTrainingPackage> personalTrainingPackages, int totalPage) =
                planTemplateRepository.GetPersionalTrainingPackage(getPlanDto);
            ResponseGetPersonalTrainingPackageListDto response = new ResponseGetPersonalTrainingPackageListDto()
            {
                PersonalTrainingPackageList = mapper.Map<List<ResponseGetPersonalTrainingPackageDto>>(personalTrainingPackages),
                TotalPage = totalPage,
            };
            return response;
        }

        /// <summary>
        /// 取得票劵方案
        /// </summary>
        public ResponseGetTicketPlanListDto GetTicketPlan(RequestGetPlanDto getPlanDto)
        {
            (List<TicketPlan> ticketPlans, int totalPage) = planTemplateRepository.GetTicketPlan(getPlanDto);
            ResponseGetTicketPlanListDto response = new ResponseGetTicketPlanListDto()
            {
                TicketPlanList = mapper.Map<List<ResponseGetTicketPlanDto>>(ticketPlans),
                TotalPage = totalPage,
            };
            return response;
        }
    }
}