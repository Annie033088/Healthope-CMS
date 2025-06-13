using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Invoice;
using ApiLayer.Models.Invoice.Request;
using ApiLayer.Models.Invoice.Response;
using ApiLayer.Models.Job;
using AutoMapper;
using DomainLayer.Models;
using Newtonsoft.Json;
using NLog;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace ApiLayer.Service
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IMapper mapper;
        private readonly IInvoiceRepository invoiceRepository;
        private readonly IHttpService httpService;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public InvoiceService(IMapper mapper, IInvoiceRepository invoiceRepository, IHttpService httpService)
        {
            this.mapper = mapper;
            this.invoiceRepository = invoiceRepository;
            this.httpService = httpService;
        }

        /// <summary>
        /// 新增發票字軌
        /// </summary>
        public bool AddInvoiceTrackNumber(RequestAddInvoiceTrackNumberDto addInvoiceTrackNumberDto)
        {
            try
            {
                InvoiceTrackNumber trackNumber = mapper.Map<InvoiceTrackNumber>(addInvoiceTrackNumberDto);
                return invoiceRepository.AddInvoiceTrackNumber(trackNumber);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得字軌
        /// </summary>
        public ResponseGetInvoiceTrackNumberListDto GetInvoiceTrackNumber(RequestGetInvoiceTrackNumberDto getInvoiceTrackNumberDto)
        {
            try
            {
                (List<InvoiceTrackNumber> invoiceTrackNumbers, int totalPage)
                    = invoiceRepository.GetInvoiceTrackNumber(getInvoiceTrackNumberDto);
                ResponseGetInvoiceTrackNumberListDto response = new ResponseGetInvoiceTrackNumberListDto()
                {
                    InvoiceTrackNumberList = mapper.Map<List<ResponseGetInvoiceTrackNumberDto>>(invoiceTrackNumbers),
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
        /// 修改字軌狀態
        /// </summary>
        public ErrorCodeDefine EditInvoiceTrackNumberStatus(RequestEditInvoiceTrackNumberStatusDto editInvoiceTrackNumberStatusDto)
        {
            try
            {
                InvoiceTrackNumber invoiceTrackNumber = mapper.Map<InvoiceTrackNumber>(editInvoiceTrackNumberStatusDto);
                int errorCodeNumber = invoiceRepository.EditInvoiceTrackNumberStatus(invoiceTrackNumber);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumber)) return (ErrorCodeDefine.ServerError);

                ErrorCodeDefine errorCode = (ErrorCodeDefine)errorCodeNumber;
                return errorCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 刪除字軌
        /// </summary>
        public bool DeleteInvoiceTrackNumber(InvoiceTrackNumberIdDto invoiceTrackNumberIdDto)
        {
            try
            {
                return invoiceRepository.DeleteInvoiceTrackNumber(invoiceTrackNumberIdDto.InvoiceTrackNumberId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 請求開立發票
        /// </summary>
        public Task PrintInvoice(RequestPrintInvoiceDto requestPrintInvoiceDto)
        {
            try
            {
                string url = "https://localhost:44395/Invoice/PrintInvoice";
                Dictionary<string, string> dictionaryContent = new Dictionary<string, string>
                {
                    { "ProductName", requestPrintInvoiceDto.PlanName },
                    { "RandomNumber", requestPrintInvoiceDto.RandomNumber },
                    { "InvoiceNumber", requestPrintInvoiceDto.InvoiceNumber },
                    { "Amount", requestPrintInvoiceDto.TotalAmount.ToString() },
                };
                string json = JsonConvert.SerializeObject(dictionaryContent);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                string responseString = httpService.SendPost(url, content);

                ResponsePrintInvoiceDto response = JsonConvert.DeserializeObject<ResponsePrintInvoiceDto>(responseString);
                PrintInvoiceErrorCode responseCode = (PrintInvoiceErrorCode)(int.Parse(response.Code));

                // 根據是否為系統延時問題決定要不要修改發票狀態
                if (responseCode != PrintInvoiceErrorCode.SystemOccupied)
                {
                    bool successFlag;

                    // 開立發票 成功/失敗
                    if (responseCode == PrintInvoiceErrorCode.Success)
                    {
                        successFlag = EditElectronicInvoiceStatus(
                            true, requestPrintInvoiceDto.ElectronicInvoiceId, response.InoviceTime);
                    }
                    else
                    {
                        successFlag = EditElectronicInvoiceStatus(
                          false, requestPrintInvoiceDto.ElectronicInvoiceId, string.Empty);
                    }

                    if (!successFlag) logger.Error("修改發票狀態失敗");
                }

                string errorText = string.Empty;

                // 需要重試的情況
                if (responseCode == PrintInvoiceErrorCode.SystemOccupied)
                {
                    errorText = "系統忙碌中, 請稍後再試";
                    throw new Exception("開立發票失敗:" + errorText);
                }
                // 單純紀錄 log
                else if (responseCode == PrintInvoiceErrorCode.BuyerNumberError)
                {
                    errorText = "買方統編錯誤";
                    logger.Error(errorText);
                }
                else if (responseCode == PrintInvoiceErrorCode.Used)
                {
                    errorText = "發票已使用";
                    logger.Error(errorText);
                }
                else if (responseCode == PrintInvoiceErrorCode.InsufficientNumberRange)
                {
                    errorText = "號碼區間不足";
                    logger.Error(errorText);
                }

                return Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改電子發票狀態
        /// </summary>
        public bool EditElectronicInvoiceStatus(bool success, int electronicInvoiceId, string invocieTime)
        {
            try
            {
                return invoiceRepository.EditElectronicInvoiceStatus(success, electronicInvoiceId, invocieTime);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}