using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ApiLayer.Interface;
using ApiLayer.Job;
using ApiLayer.Models;
using ApiLayer.Models.Invoice;
using ApiLayer.Models.Job;
using ApiLayer.Models.Other;
using DomainLayer.Models;
using Newtonsoft.Json;
using NLog;
using PersistentLayer.Interface;
using PersistentLayer.Models;
using static Hangfire.Storage.JobStorageFeatures;

namespace ApiLayer.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IHttpService httpService;
        private readonly ITransactionRepository transactionRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IJobDispatcher jobDispatcher;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public PaymentService(IHttpService httpService, ITransactionRepository transactionRepository,
            IOrderRepository orderRepository, IJobDispatcher jobDispatcher)
        {
            this.httpService = httpService;
            this.transactionRepository = transactionRepository;
            this.orderRepository = orderRepository;
            this.jobDispatcher = jobDispatcher;
        }

        /// <summary>
        /// 刷卡
        /// </summary>
        public async Task<(ErrorCodeDefine errorCode, DBResponsePaymentDto dbResponse)> PayByCard(
            RequestCardPaymentDto requestCardPaymentDto, int creditCardTransactionId, RequestPayByCardDto payByCardDto)
        {
            try
            {
                string url = "https://localhost:44395/Payment/Card";
                Dictionary<string, string> dictionaryContent = new Dictionary<string, string>
                {
                    { "OrderId", requestCardPaymentDto.OrderId.ToString() },
                    { "Amount", requestCardPaymentDto.Amount.ToString() },
                    { "TransactionId", requestCardPaymentDto.TransactionId },
                };
                string json = JsonConvert.SerializeObject(dictionaryContent);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                TimeSpan payByCardTime = TimeSpan.FromSeconds(30);
                string responseString = await httpService.SendPostAsync(url, content, payByCardTime);

                ResponseCardPaymentDto response = JsonConvert.DeserializeObject<ResponseCardPaymentDto>(responseString);

                CreditCardTransaction transaction = new CreditCardTransaction
                {
                    CreditCardTransactionId = creditCardTransactionId,
                    AuthCode = response.AuthCode,
                    CardLastFour = response.CardLastFour,
                    CardType = response.CardType,
                    TransactionId = response.TransactionId,
                };

                // 查看回傳是成功/失敗
                if (response.Status)
                {
                    bool editTransactionFlag = transactionRepository.EditCreditCardTransactionStatusSuccess(transaction);

                    if (!editTransactionFlag)
                    {
                        logger.Fatal("刷卡成功但交易紀錄更新失敗!");
                        return (ErrorCodeDefine.CardPaySuccessTransactionUpdateFail, null);
                    }

                    (int errorCodeNumber, DBResponsePaymentDto dbResponse) = orderRepository.PayByCardSuccess(payByCardDto);

                    if (errorCodeNumber != (int)ErrorCodeDefine.Success)
                    {
                        logger.Fatal("刷卡及交易紀錄更新成功，但修改訂單失敗!");
                        return (ErrorCodeDefine.TransactionSuccessOrderUpdateFail, null);
                    }

                    RequestPrintInvoiceDto printInvoiceDto = new RequestPrintInvoiceDto()
                    {
                        ElectronicInvoiceId = dbResponse.ElectronicInvoiceId,
                        InvoiceNumber = dbResponse.InvoiceNumber,
                        PlanName = dbResponse.PlanName,
                        RandomNumber = dbResponse.RandomNumber,
                        TotalAmount = dbResponse.TotalAmount,
                    };
                    jobDispatcher.Enqueue<RequestPrintInoviceJob, RequestPrintInvoiceDto>(printInvoiceDto);

                    return (ErrorCodeDefine.Success, dbResponse);
                }

                bool successEditStatusFail = transactionRepository.EditCreditCardTransactionStatusFail(creditCardTransactionId);

                if (!successEditStatusFail) logger.Fatal("交易失敗但修改交易紀錄及訂單狀態失敗! 會導致該筆訂單無法重試付款");

                return (ErrorCodeDefine.PayFailed, null);
            }
            catch (TimeoutException)
            {
                transactionRepository.EditCreditCardTransactionStatusFail(creditCardTransactionId);
                throw;
            }
            catch (HttpRequestException)
            {
                transactionRepository.EditCreditCardTransactionStatusFail(creditCardTransactionId);
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}