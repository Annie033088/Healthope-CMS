using System.Threading.Tasks;

namespace ApiLayer.Interface
{
    public interface IEmailService
    {
        /// <summary>
        /// 寄出 mail
        /// </summary>
        Task SendEmail(string recipient, string subject, string htmlBody);
    }
}
