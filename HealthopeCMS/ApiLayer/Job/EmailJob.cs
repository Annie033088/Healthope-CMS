using System;
using ApiLayer.Interface;
using NLog;

namespace ApiLayer.Job
{
    public class EmailJob : IEmailJob
    {
        private readonly IEmailService emailService;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public EmailJob(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public void SendEmail(string recipient, string subject, string body)
        {
            try
            {
                emailService.SendEmail(recipient, subject, body);
            }
            catch (Exception ex)
            {
                logger.Error($"SendEmailJob failed once: {ex.Message}");
                throw;
            }
        }
    }
}