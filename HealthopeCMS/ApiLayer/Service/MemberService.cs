using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApiLayer.Interface;
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
        private readonly IMapper mapper;

        public MemberService(IMemberRepository memberRepository, IMapper mapper)
        {
            this.memberRepository = memberRepository;
            this.mapper = mapper;
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
    }
}