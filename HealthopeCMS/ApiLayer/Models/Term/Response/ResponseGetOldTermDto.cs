namespace ApiLayer.Models.Term.Response
{
    public class ResponseGetOldTermDto
    {
        /// <summary>
        /// 條款 Id
        /// </summary>
        public int TermId { get; set; }

        /// <summary>
        /// 版本號
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 內文
        /// </summary>
        public string DetailContent { get; set; }
    }
}