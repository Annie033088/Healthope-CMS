namespace ApiLayer.Models.Job
{
    public class SendEmailDto
    {
        /// <summary>
        /// 收件人
        /// </summary>
        public string Recipient { get; set; }

        /// <summary>
        /// 主旨
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public string Body { get; set; }
    }
}