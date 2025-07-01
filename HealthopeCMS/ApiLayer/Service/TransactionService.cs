using System;
using System.Collections.Generic;
using ApiLayer.Interface;
using ApiLayer.Models.Transaction.Request;
using ApiLayer.Models.Transaction.Response;
using AutoMapper;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace ApiLayer.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IMapper mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            this.transactionRepository = transactionRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得付款紀錄
        /// </summary>
        public ResponseGetTransactionListDto GetTransaction(RequestGetTransactionDto getTransactionDto)
        {
            try
            {
                (List<PaymentTransaction> transactions, int totalPage) = transactionRepository.GetTransaction(getTransactionDto);
                ResponseGetTransactionListDto responseGetTransactionList = new ResponseGetTransactionListDto()
                {
                    TransactionList = mapper.Map<List<ResponseGetTransactionDto>>(transactions),
                    TotalPage = totalPage
                };

                return responseGetTransactionList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得金流資訊(Auth code 跟 外部金流Id)
        /// </summary>
        public ResponsetGetCreditCardCashFlowDto GetCreditCardCashFlowData(RequestTransactionIdDto transactionIdDto)
        {
            try
            {
                PaymentTransaction transaction = transactionRepository.GetCreditCardCashFlowData(transactionIdDto.TransactionId);
                ResponsetGetCreditCardCashFlowDto responsetGet = mapper.Map<ResponsetGetCreditCardCashFlowDto>(transaction);

                return responsetGet;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}