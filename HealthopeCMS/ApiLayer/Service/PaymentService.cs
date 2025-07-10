using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Interface;
using ApiLayer.Job;
using ApiLayer.Models;
using ApiLayer.Models.Job;
using ApiLayer.Models.Other;
using DomainLayer.Models;
using Newtonsoft.Json;
using NLog;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace ApiLayer.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IHttpService httpService;
        private readonly ITransactionRepository transactionRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IJobDispatcher jobDispatcher;
        private readonly IInvoiceRepository invoiceRepository;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public PaymentService(IHttpService httpService, ITransactionRepository transactionRepository,
            IOrderRepository orderRepository, IJobDispatcher jobDispatcher, IInvoiceRepository invoiceRepository)
        {
            this.httpService = httpService;
            this.transactionRepository = transactionRepository;
            this.orderRepository = orderRepository;
            this.jobDispatcher = jobDispatcher;
            this.invoiceRepository = invoiceRepository;
        }

        /// <summary>
        /// 刷卡
        /// </summary>
        public async Task<(ErrorCodeDefine errorCode, DBResponseSingleEntryPassDto dBResponseSingleEntryPassDto)> PayByCard(
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
                    // 更新交易狀態
                    (int errorCodeNumber, DBResponseSingleEntryPassDto dBResponseSingleEntryPassDto) =
                        orderRepository.PayByCardSuccess(payByCardDto, transaction);

                    if (errorCodeNumber != (int)ErrorCodeDefine.Success)
                    {
                        logger.Error("交易成功，但交易紀錄及訂單狀態修改失敗!");
                        return (ErrorCodeDefine.CardPaySuccessTransactionUpdateFail, null);
                    }

                    // 取得發票資料
                    (int invoiceErrorCodeNumber, DBResponsePrintInvoiceDto responsePrintInvoiceDto) =
                        invoiceRepository.GetInvoiceNumberAndAddElectronicInvoice(payByCardDto.OrderId);

                    if (invoiceErrorCodeNumber != (int)ErrorCodeDefine.Success)
                    {
                        logger.Error("交易紀錄更新成功，但列印發票失敗!");
                        return (ErrorCodeDefine.TransactionSuccessPrintInvoiceFail, null);
                    }

                    RequestPrintInvoiceDto printInvoiceDto = new RequestPrintInvoiceDto()
                    {
                        ElectronicInvoiceId = responsePrintInvoiceDto.ElectronicInvoiceId,
                        InvoiceNumber = responsePrintInvoiceDto.InvoiceNumber,
                        PlanName = responsePrintInvoiceDto.PlanName,
                        RandomNumber = responsePrintInvoiceDto.RandomNumber,
                        TotalAmount = responsePrintInvoiceDto.TotalAmount,
                    };
                    jobDispatcher.Enqueue<RequestPrintInoviceJob, RequestPrintInvoiceDto>(printInvoiceDto);

                    return (ErrorCodeDefine.Success, dBResponseSingleEntryPassDto);
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