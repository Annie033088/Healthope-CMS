using System;
using System.Threading.Tasks;
using ApiLayer.Interface;
using ApiLayer.Models.Job;
using Hangfire;
using NLog;

namespace ApiLayer.Job
{
    [AutomaticRetry(Attempts = 3)]
    public class SendEmailJob : IJob<SendEmailDto>
    {
        private readonly IEmailService emailService;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public SendEmailJob(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public async Task Execute(SendEmailDto sendEmailDto)
        {
            try
            {
                await emailService.SendEmail(sendEmailDto.Recipient, sendEmailDto.Subject, sendEmailDto.Body);
            }
            catch (Exception ex)
            {
                logger.Error($"SendEmailJob failed once: {ex.Message}");
                throw;
            }
        }
    }
}