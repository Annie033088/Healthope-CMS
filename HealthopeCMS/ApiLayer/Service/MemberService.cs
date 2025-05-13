using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Admin.ResponseAdminDto;
using ApiLayer.Models.Member;
using ApiLayer.Models.Member.Response;
using AutoMapper;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;
using PersistentLayer.Repository;

namespace ApiLayer.Service
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository memberRepository;
        private readonly IRedisService redisService;
        private readonly IMapper mapper;

        public MemberService(IMemberRepository memberRepository, IMapper mapper, IRedisService redisService)
        {
            this.memberRepository = memberRepository;
            this.mapper = mapper;
            this.redisService = redisService;
        }

        /// <summary>
        /// 取得會員列表
        /// </summary>
        public ResponseGetMemberListDto GetMember(RequestGetMemberDto getMemberDto)
        {
            try
            {
                (List<Member> members, int totalPage) = memberRepository.GetMember(getMemberDto);
                ResponseGetMemberListDto responseGetMemberDto = new ResponseGetMemberListDto()
                {
                    MemberList = mapper.Map<List<ResponseGetMemberDto>>(members),
                    TotalPage = totalPage
                };

                return responseGetMemberDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根據 id 取得修改會員時需要的資料
        /// </summary>
        public ResponseGetMemberEditDataByIdDto GetMemberEditDataById(RequestMemberIdDto getMemberByIdDto)
        {
            try
            {
                Member member = memberRepository.GetMemberEditDataById(getMemberByIdDto.MemberId);
                ResponseGetMemberEditDataByIdDto response = mapper.Map<ResponseGetMemberEditDataByIdDto>(member);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改會員手機或狀態
        /// </summary>
        public ErrorCodeDefine EditMember(RequestEditMemberDto editMemberDto)
        {
            try
            {
                int errorCodeNumber = memberRepository.EditMember(editMemberDto);

                // 如果沒有被定義在 enum 裡
                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumber)) return ErrorCodeDefine.ServerError;

                ErrorCodeDefine errorCode = (ErrorCodeDefine)errorCodeNumber;

                // 如果修改狀態成功，根據修改的狀態，清除會員 redis 會話
                if (errorCode == ErrorCodeDefine.Success)
                {
                    // 該會員被禁用
                    if (editMemberDto.Status != null && editMemberDto.Status == false)
                    {
                        string memberRedisKey = "Member" + editMemberDto.MemberId;
                        redisService.DeleteKey(memberRedisKey);
                    }
                }

                return errorCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得會員詳細資料
        /// </summary>
        public ResponseGetMemberDetailDto GetMemberDetail(RequestMemberIdDto memberIdDto)
        {
            try
            {
                Member member = memberRepository.GetMemberDetail(memberIdDto.MemberId);
                return mapper.Map<ResponseGetMemberDetailDto>(member);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}