namespace ApiLayer.Models.Term.Request
{
    public class RequestAddTermDto
    {
        /// <summary>
        /// 內文
        /// </summary>
        public string DetailContent { get; set; }

        /// <summary>
        /// 類型
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 適用對象
        /// </summary>
        public byte ApplicableTarget { get; set; }

        /// <summary>
        /// 描述更新內容
        /// </summary>
        public string VersionDescription { get; set; }
    }
}