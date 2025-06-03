using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using ApiLayer.Interface;
using DomainLayer.Utility;
using DomainLayer.Interface;

namespace ApiLayer.Service
{
    public class EmailService : IEmailService
    {
        private readonly IAppSetting appSetting;

        /// <summary>
        /// SMTP 郵件伺服器主機名稱
        /// </summary>
        private readonly string smtpHost;

        /// <summary>
        /// SMTP 主機的連接埠號
        /// </summary>
        private readonly int smtpPort;

        /// <summary>
        /// Gmaul
        /// </summary>
        private readonly string fromEmail;

        /// <summary>
        /// 登入 SMTP 郵件主機的密碼
        /// </summary>
        private readonly string fromPassword;

        public EmailService(IAppSetting appSetting)
        {
            this.appSetting = appSetting;
            this.smtpHost = this.appSetting.GetConfigurationAppsetting("SmtpHost");
            this.smtpPort = int.Parse(this.appSetting.GetConfigurationAppsetting("SmtpPort"));
            this.fromEmail = this.appSetting.GetConfigurationAppsetting("FromEmail");
            this.fromPassword = this.appSetting.GetConfigurationAppsetting("FromPassword");
        }

        /// <summary>
        /// 寄出 mail
        /// </summary>
        public void SendEmail(string recipient, string subject, string htmlBody)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(fromEmail));
            message.To.Add(MailboxAddress.Parse(recipient));
            message.Subject = subject;

            // 使用 HTML 郵件內容
            message.Body = new TextPart("html") { Text = htmlBody };

            try
            {
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Connect(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                    smtp.Authenticate(fromEmail, fromPassword);
                    smtp.Send(message);
                    smtp.Disconnect(true);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}