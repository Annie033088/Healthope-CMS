using System;
using System.Collections.Generic;
using ApiLayer.Interface;
using ApiLayer.Models.Term.Request;
using ApiLayer.Models.Term.Response;
using AutoMapper;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace ApiLayer.Service
{
    public class TermService : ITermService
    {
        private readonly IMapper mapper;
        private readonly ITermRepository termRepository;

        public TermService(IMapper mapper, ITermRepository termRepository)
        {
            this.mapper = mapper;
            this.termRepository = termRepository;
        }

        /// <summary>
        /// 取得舊條款
        /// </summary>
        public List<ResponseGetOldTermDto> GetOldTerm(RequestGetOldTermDto getOldTerm)
        {
            try
            {
                Term getTerm = mapper.Map<Term>(getOldTerm);
                List<Term> terms = termRepository.GetOldTerm(getTerm);
                List<ResponseGetOldTermDto> response = mapper.Map<List<ResponseGetOldTermDto>>(terms);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 新增條款
        /// </summary>
        public bool AddTerm(RequestAddTermDto addTermDto)
        {
            try
            {
                Term term = mapper.Map<Term>(addTermDto);
                return termRepository.AddTerm(term);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得條款
        /// </summary>
        public ResponseGetTermListDto GetTerm(RequestGetTermDto getTermDto)
        {
            try
            {
                (List<Term> terms, int totalPage) = termRepository.GetTerm(getTermDto);
                ResponseGetTermListDto response = new ResponseGetTermListDto()
                {
                    TermList = mapper.Map<List<ResponseGetTermDto>>(terms),
                    TotalPage = totalPage,
                };
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}