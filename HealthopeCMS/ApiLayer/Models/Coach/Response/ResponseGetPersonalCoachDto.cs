namespace ApiLayer.Models.Coach.Response
{
    public class ResponseGetPersonalCoachDto
    {
        /// <summary>
        /// 教練 ID，主鍵
        /// </summary>
        public int CoachId { get; set; }

        /// <summary>
        /// 教練姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        public int Phone { get; set; }
    }
}