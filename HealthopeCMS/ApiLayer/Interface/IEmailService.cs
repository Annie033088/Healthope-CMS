using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLayer.Interface
{
    public interface IEmailService
    {
        /// <summary>
        /// 寄出 mail
        /// </summary>
        void SendEmail(string recipient, string subject, string htmlBody);
    }
}
