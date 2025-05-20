namespace ApiLayer.Models.Other
{
    public class FileDto
    {
        /// <summary>
        /// 文件檔 ( 二進位 )
        /// </summary>
        public byte[] FileData;

        /// <summary>
        /// mime 類型
        /// </summary>
        public string MimeType { get; set; }
    }
}