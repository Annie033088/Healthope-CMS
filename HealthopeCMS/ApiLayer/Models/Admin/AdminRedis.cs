namespace ApiLayer.Models.Admin
{
    public class AdminRedis
    {
        /// <summary>
        /// redis 存的 sessionId
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 檢查 權限/狀態 是否被異動
        /// </summary>
        public ErrorCodeDefine ErrorCode { get; set; }
    }
}