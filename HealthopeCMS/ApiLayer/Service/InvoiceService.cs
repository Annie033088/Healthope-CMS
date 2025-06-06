using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Invoice.Request;
using ApiLayer.Models.Invoice.Response;
using AutoMapper;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace ApiLayer.Service
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IMapper mapper;
        private readonly IInvoiceRepository invoiceRepository;

        public InvoiceService(IMapper mapper, IInvoiceRepository invoiceRepository)
        {
            this.mapper = mapper;
            this.invoiceRepository = invoiceRepository;
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
    }
}