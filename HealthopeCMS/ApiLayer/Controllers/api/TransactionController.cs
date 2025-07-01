using System;
using System.Web.Http;
using ApiLayer.Filters;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Transaction;
using ApiLayer.Models.Transaction.Request;
using ApiLayer.Models.Transaction.Response;
using DomainLayer.Utility;
using NLog;
using PersistentLayer.Models;

namespace ApiLayer.Controllers.api
{
    [RequestLoggerFilter]
    [VeriyLoginFilter]
    [AdminPermissionAuthFilter]
    public class TransactionController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ITransactionService transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        /// <summary>
        /// 取得付款紀錄
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetTransaction([FromBody] RequestGetTransactionDto getTransactionDto)
        {
            try
            {
                FormatValidation formatValidation = new FormatValidation();
                // 驗證前端傳遞的參數是否合法
                bool modelValidFlag = true;

                if (!ModelState.IsValid)
                    modelValidFlag = false;
                if (getTransactionDto.Method != null && !Enum.IsDefined(typeof(TransactionMethod), getTransactionDto.Method))
                    modelValidFlag = false;
                if (getTransactionDto.Status != null && !Enum.IsDefined(typeof(TransactionStatus), getTransactionDto.Status))
                    modelValidFlag = false;
                if (!((getTransactionDto.SortOrder == "ascending") || (getTransactionDto.SortOrder == "descending")))
                    modelValidFlag = false;
                if (!((getTransactionDto.SortOption == "amount") || (getTransactionDto.SortOption == "time")
                    || (getTransactionDto.SortOption == null)))
                    modelValidFlag = false;
                if (!((getTransactionDto.RecordPerPage == 8) || (getTransactionDto.RecordPerPage == 12)
                    || (getTransactionDto.RecordPerPage == 16)))
                    modelValidFlag = false;
                if (getTransactionDto.Page < 1)
                    modelValidFlag = false;

                ResultResponse response;

                // 格式錯誤
                if (!modelValidFlag)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetTransactionListDto responseGet = transactionService.GetTransaction(getTransactionDto);
                response = new ResultResponse<ResponseGetTransactionListDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = responseGet
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                return Ok(response);
            }
        }

        /// <summary>
        /// 取得金流資訊(Auth code 跟 外部金流Id)
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetCreditCardCashFlowData([FromBody] RequestTransactionIdDto transactionIdDto)
        {
            try
            {
                ResultResponse response;

                // 格式錯誤
                if (!ModelState.IsValid
                    || transactionIdDto.TransactionId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponsetGetCreditCardCashFlowDto responsetGet = transactionService.GetCreditCardCashFlowData(transactionIdDto);
                response = new ResultResponse<ResponsetGetCreditCardCashFlowDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = responsetGet,
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                return Ok(response);
            }
        }
    }
}
