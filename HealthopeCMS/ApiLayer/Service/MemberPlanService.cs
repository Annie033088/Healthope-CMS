using System;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.MemberPlan.Request;
using AutoMapper;
using DomainLayer.Models;
using PersistentLayer.Interface;

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

        /// <summary>
        /// 修改會籍狀態
        /// </summary>
        public ErrorCodeDefine EditMemberMembershipPlanStatus(RequestMemberMembershipPlanStatusDto editMemberMembershipPlanStatusDto)
        {
            try
            {
                MemberMembershipPlan memberMembershipPlan = mapper.Map<MemberMembershipPlan>(editMemberMembershipPlanStatusDto);
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

        /// <summary>
        /// 修改教練課方案的教練
        /// </summary>
        public ErrorCodeDefine EditMemberPersonalTrainingPackageCoach(RequestEditMemberPersonalTrainingPackageCoachDto editMemberPersonalTrainingPackageCoachDto)
        {
            try
            {
                MemberPersonalTrainingPackage memberPersonalTrainingPackage =
                    mapper.Map<MemberPersonalTrainingPackage>(editMemberPersonalTrainingPackageCoachDto);
                int errorCodeNumber = memberPlanRepository.EditMemberPersonalTrainingPackageCoach(memberPersonalTrainingPackage);

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