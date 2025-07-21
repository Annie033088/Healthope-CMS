using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Member;
using ApiLayer.Models.MemberClass.Request;
using AutoMapper;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace ApiLayer.Service
{
    public class MemberClassService : IMemberClassService
    {
        private readonly IMemberClassRepository memberClassRepository;
        private readonly IMapper mapper;

        public MemberClassService(IMemberClassRepository memberClassRepository, IMapper mapper)
        {
            this.memberClassRepository = memberClassRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得新增教練課時的教練課跟教練資料
        /// </summary>
        public List<ResponseGetPersonalTrainingPackageAndCoachDto> GetPersonalTrainingPackageAndCoach(RequestMemberIdDto memberIdDto)
        {
            try
            {
                return memberClassRepository.GetPersonalTrainingPackageAndCoach(memberIdDto.MemberId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 新增會員預約教練課程
        /// </summary>
        public ErrorCodeDefine AddMemberPersonalClass(RequestAddMemberPersonalClassDto addMemberPersonalClassDto)
        {
            try
            {
                MemberPersonalClass memberPersonalClass = mapper.Map<MemberPersonalClass>(addMemberPersonalClassDto);
                int errorCodeNumber = memberClassRepository.AddMemberPersonalClass(memberPersonalClass);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumber))
                {
                    return (ErrorCodeDefine.ServerError);
                }

                return (ErrorCodeDefine)errorCodeNumber;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得會員預約的教練課程列表
        /// </summary>
        public ResponseGetMemberPersonalClassListDto GetMemberPersonalClass(RequestGetMemberPersonalClassDto getMemberPersonalClassDto)
        {
            try
            {
                return memberClassRepository.GetMemberPersonalClass(getMemberPersonalClassDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改預約課程備註
        /// </summary>
        public bool EditMemberPersonalClassRemark(RequestEditMemberPersonalClassRemarkDto editMemberPersonalClassRemarkDto)
        {
            try
            {
                MemberPersonalClass memberPersonalClass = mapper.Map<MemberPersonalClass>(editMemberPersonalClassRemarkDto);
                return memberClassRepository.EditMemberPersonalClassRemark(memberPersonalClass);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改會員的教練預約課程狀態
        /// </summary>
        public ErrorCodeDefine EditMemberPersonalClassStatus(RequestEditMemberPersonalClassStatusDto editStatusDto)
        {
            try
            {
                MemberPersonalClass memberPersonalClass = mapper.Map<MemberPersonalClass>(editStatusDto);
                int errorCodeNumber = memberClassRepository.EditMemberPersonalClassStatus(memberPersonalClass);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumber))
                {
                    return (ErrorCodeDefine.ServerError);
                }

                return (ErrorCodeDefine)errorCodeNumber;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 每日取消當日預約中的教練課程 (預約中課程於一天之前 無確認，即改為取消)
        /// </summary>
        public Task AutoCancelReservingMemberPersonalClass()
        {
            try
            {
               return memberClassRepository.AutoCancelReservingMemberPersonalClass();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}