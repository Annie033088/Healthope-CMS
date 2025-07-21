using System;
using System.Collections.Generic;
using ApiLayer.Interface;
using AutoMapper;
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
                (List<ResponseGetRefundDto> refunds, int totalPage) = refundRepository.GetRefund(getRefundDto);
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