using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApiLayer.Interface;
using ApiLayer.Job;
using ApiLayer.Models;
using ApiLayer.Models.LeaseAgreement.Request;
using ApiLayer.Models.LeaseAgreement.Response;
using ApiLayer.Models.Term.Response;
using AutoMapper;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;
using PersistentLayer.Repository;

namespace ApiLayer.Service
{
    public class LeaseAgreementService : ILeaseAgreementService
    {
        private readonly IMapper mapper;
        private readonly ILeaseAgreementRepository leaseAgreementRepository;
        private readonly IEmailJob emailJob;

        public LeaseAgreementService(IMapper mapper, ILeaseAgreementRepository leaseAgreementRepository, IEmailJob emailJob)
        {
            this.mapper = mapper;
            this.leaseAgreementRepository = leaseAgreementRepository;
            this.emailJob = emailJob;
        }

        /// <summary>
        /// 新增條款
        /// </summary>
        public bool AddLeaseAgreement(RequestAddLeaseAgreementDto addLeaseAgreementDto)
        {
            try
            {
                LeaseAgreement leaseAgreement = mapper.Map<LeaseAgreement>(addLeaseAgreementDto);
                return leaseAgreementRepository.AddLeaseAgreement(leaseAgreement);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得條款
        /// </summary>
        public ResponseGetLeaseAgreementListDto GetLeaseAgreement(RequestGetLeaseAgreementDto getLeaseAgreementDto)
        {
            try
            {
                (List<LeaseAgreement> leaseAgreements, int totalPage) = leaseAgreementRepository.GetLeaseAgreement(getLeaseAgreementDto);
                ResponseGetLeaseAgreementListDto response = new ResponseGetLeaseAgreementListDto()
                {
                    LeaseAgreementList = mapper.Map<List<ResponseGetLeaseAgreementDto>>(leaseAgreements),
                    TotalPage = totalPage,
                };
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改租約狀態 (僅限未啟用=>啟用, 啟用=>已完成、取消)
        /// </summary>
        public ErrorCodeDefine EditLeaseAgreementStatus(RequestEditLeaseAgreementStatusDto editLeaseAgreementStatusDto)
        {
            try
            {
                LeaseAgreement leaseAgreement = mapper.Map<LeaseAgreement>(editLeaseAgreementStatusDto);

                (int errorCodeNumber, bool sendEmailFlag) = leaseAgreementRepository.EditLeaseAgreementStatus(leaseAgreement);
                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumber))
                    return ErrorCodeDefine.ServerError;

                ErrorCodeDefine errorCode = (ErrorCodeDefine)errorCodeNumber;

                if (errorCode == ErrorCodeDefine.Success && sendEmailFlag) {
                    emailJob.SendEmail("", "", "");
                }
                return errorCode;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}