using System;
using System.Collections.Generic;
using ApiLayer.Interface;
using ApiLayer.Models.Refund.Response;
using AutoMapper;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace ApiLayer.Service
{
    public class RefundService : IRefundService
    {
        private readonly IRefundRepository refundRepository;
        private readonly IMapper mapper;

        public RefundService(IRefundRepository refundRepository, IMapper mapper)
        {
            this.refundRepository = refundRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得退款紀錄
        /// </summary>
        public ResponseGetRefundListDto GetRefund(RequestGetRefundDto getRefundDto)
        {
            try
            {
                (List<Refund> refunds, int totalPage) = refundRepository.GetRefund(getRefundDto);
                ResponseGetRefundListDto response = new ResponseGetRefundListDto
                {
                    RefundList = refunds,
                    TotalPage = totalPage
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