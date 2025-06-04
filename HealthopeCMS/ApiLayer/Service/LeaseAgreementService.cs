using System;
using System.Collections.Generic;
using ApiLayer.Interface;
using ApiLayer.Job;
using ApiLayer.Models;
using ApiLayer.Models.Job;
using ApiLayer.Models.LeaseAgreement.Request;
using ApiLayer.Models.LeaseAgreement.Response;
using AutoMapper;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace ApiLayer.Service
{
    public class LeaseAgreementService : ILeaseAgreementService
    {
        private readonly IMapper mapper;
        private readonly ILeaseAgreementRepository leaseAgreementRepository;
        private readonly IJobDispatcher jobDispatcher;


        public LeaseAgreementService(IMapper mapper, ILeaseAgreementRepository leaseAgreementRepository,
            IJobDispatcher jobDispatcher)
        {
            this.mapper = mapper;
            this.leaseAgreementRepository = leaseAgreementRepository;
            this.jobDispatcher = jobDispatcher;
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

                (int errorCodeNumber, bool sendEmailFlag, DateTime leaseEndTime) =
                    leaseAgreementRepository.EditLeaseAgreementStatus(leaseAgreement);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumber) ||
                    (sendEmailFlag && leaseEndTime == DateTime.MinValue))
                    return ErrorCodeDefine.ServerError;

                ErrorCodeDefine errorCode = (ErrorCodeDefine)errorCodeNumber;

                if (errorCode == ErrorCodeDefine.Success && sendEmailFlag)
                {
                    SendEmailDto sendEmailDto = new SendEmailDto()
                    {
                        Recipient = "Annie890308@gmail.com",
                        Subject = "場館租約即將到期通知",
                        Body = string.Format(@"
                        <p>您好，</p>
                        <p>根據系統資料，以下場館的租約即將於 {0} 到期，特此提醒您</p>
                        <p>請您視情況通知相關人員或進行後續處理。如資訊有誤，歡迎與我們聯繫協助更新。</p>
                        <p>感謝您的留意與配合。</p>
                        <p>敬祝<br>順心如意</p>
                        <p>{1}<br>{2}</p>
                        ", leaseEndTime.Date, "小卷", "fengmu9966@gmail.com")
                    };
                    jobDispatcher.Enqueue<SendEmailJob, SendEmailDto>(sendEmailDto);
                }

                return errorCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改是否提醒
        /// </summary>
        public ErrorCodeDefine EditLeaseAgreementRemind(RequestEditLeaseAgreementRemindDto editLeaseAgreementRemindDto)
        {
            try
            {
                LeaseAgreement leaseAgreement = mapper.Map<LeaseAgreement>(editLeaseAgreementRemindDto);

                int errorCodeNumber = leaseAgreementRepository.EditLeaseAgreementRemind(leaseAgreement);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumber))
                    return ErrorCodeDefine.ServerError;

                ErrorCodeDefine errorCode = (ErrorCodeDefine)errorCodeNumber;
                return errorCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 刪除租約(僅限未啟用租約)
        /// </summary>
        public bool DeleteLeaseAgreement(RequestLeaseAgreementIdDto leaseAgreementIdDto)
        {
            try
            {
                return leaseAgreementRepository.DeleteLeaseAgreement(leaseAgreementIdDto.LeaseAgreementId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}