using System;
using System.Collections.Generic;
using System.Web.Http;
using ApiLayer.Interface;
using ApiLayer.Models;
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

        /// <summary>
        /// 取得修改條款頁面的資料
        /// </summary>
        public ResponseGetTermEditDataByIdDto GetTermEditDataById(RequestTermIdDto termIdDto)
        {
            try
            {
                Term term = termRepository.GetTermEditDataById(termIdDto.TermId);

                if (term == null) return null;

                ResponseGetTermEditDataByIdDto response = mapper.Map<ResponseGetTermEditDataByIdDto>(term);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改條款
        /// </summary>
        public ErrorCodeDefine EditTerm(RequestEditTermDto editTermDto)
        {
            try
            {
                int errorCodeNumber = termRepository.EditTerm(editTermDto);

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
        /// 修改條款狀態 (僅限草稿=>發布)
        /// </summary>
        public ErrorCodeDefine EditTermStatus(RequestEditTermStatusDto editTermStatusDto)
        {
            try
            {
                Term term = mapper.Map<Term>(editTermStatusDto);

                int errorCodeNumber = termRepository.EditTermStatus(term);
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
        /// 取得條款的詳細資料
        /// </summary>
        public ResponseGetTermDetailDto GetTermDetail(RequestTermIdDto termIdDto)
        {

            try
            {
                Term term = termRepository.GetTermDetail(termIdDto.TermId);

                if (term == null) return null;

                ResponseGetTermDetailDto response = mapper.Map<ResponseGetTermDetailDto>(term);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 刪除條款
        /// </summary>
        public bool DeleteTerm([FromBody] RequestTermIdDto termIdDto)
        {
            try
            {
                return termRepository.DeleteTerm(termIdDto.TermId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}