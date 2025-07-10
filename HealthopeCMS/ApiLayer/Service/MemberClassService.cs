using System;
using System.Collections.Generic;
using ApiLayer.Interface;
using ApiLayer.Models.Member;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace ApiLayer.Service
{
    public class MemberClassService : IMemberClassService
    {
        private readonly IMemberClassRepository memberClassRepository;

        public MemberClassService(IMemberClassRepository memberClassRepository)
        {
            this.memberClassRepository = memberClassRepository;
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
    }
}