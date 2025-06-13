namespace ApiLayer.Models.Member.Request
{
    public class RequestGetMemberByNameOrPhoneDto
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手機
        /// </summary>
        public int? Phone { get; set; }
    }
}