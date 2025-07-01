namespace ApiLayer.Models.Transaction
{
    public enum TransactionStatus : byte
    {
        /// <summary>
        /// 處理中
        /// </summary>
        Processing = 1,

        /// <summary>
        /// 成功
        /// </summary>
        Success = 2,

        /// <summary>
        /// 失敗
        /// </summary>
        Fail = 3,
    }
}