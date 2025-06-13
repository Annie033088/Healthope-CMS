using System;
using System.Threading.Tasks;
using ApiLayer.Interface;
using ApiLayer.Models.Job;
using Hangfire;
using Hangfire.Server;
using NLog;

namespace ApiLayer.Job
{
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 10, 10, 10 })]
    public class RequestPrintInoviceJob : IJob<RequestPrintInvoiceDto>
    {
        private readonly IInvoiceService invoiceService;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public RequestPrintInoviceJob(IInvoiceService invoiceService)
        {
            this.invoiceService = invoiceService;
        }

        public async Task Execute(RequestPrintInvoiceDto requestPrintInvoiceDto, PerformContext context)
        {
            try
            {
                await invoiceService.PrintInvoice(requestPrintInvoiceDto);
            }
            catch (Exception ex)
            {
                int retryCount = context.GetJobParameter<int>("RetryCount");

                if (retryCount >= 2)
                {
                    bool successFlag = invoiceService.EditElectronicInvoiceStatus(
                        false, requestPrintInvoiceDto.ElectronicInvoiceId, string.Empty);

                    if (!successFlag) logger.Error("修改發票狀態失敗");
                }

                logger.Error($"開立發票失敗: {ex.Message}");
                throw;
            }
        }
    }
}