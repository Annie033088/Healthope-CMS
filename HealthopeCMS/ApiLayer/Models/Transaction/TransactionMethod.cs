namespace ApiLayer.Models.Transaction
{
    public enum TransactionMethod : byte
    {
        /// <summary>
        /// 現金
        /// </summary>
        Cash = 1,

        /// <summary>
        /// 信用卡
        /// </summary>
        Card = 2,
    }
}