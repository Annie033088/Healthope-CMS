using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.MemberPlan.Request;
using AutoMapper;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Repository;

namespace ApiLayer.Service
{
    public class MemberPlanService : IMemberPlanService
    {
        private readonly IMemberPlanRepository memberPlanRepository;
        private readonly IMapper mapper;

        public MemberPlanService(IMemberPlanRepository memberPlanRepository, IMapper mapper)
        {
            this.memberPlanRepository = memberPlanRepository;
            this.mapper = mapper;
        }

        public ErrorCodeDefine EditMemberMembershipPlanStatus(RequestMemberMembershipPlanStatusDto addInvoiceTrackNumberDto)
        {
            try
            {
                MemberMembershipPlan memberMembershipPlan = mapper.Map<MemberMembershipPlan>(memberPlanRepository);
                int errorCodeNumber = memberPlanRepository.EditMemberMembershipPlanStatus(memberMembershipPlan);

                // 如果沒有被定義在 enum 裡
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
    }
}