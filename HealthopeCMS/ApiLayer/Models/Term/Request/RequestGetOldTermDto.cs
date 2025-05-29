namespace ApiLayer.Models.Term.Request
{
    public class RequestGetOldTermDto
    {
        /// <summary>
        /// 類型
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 適用對象
        /// </summary>
        public byte ApplicableTarget { get; set; }

    }
}