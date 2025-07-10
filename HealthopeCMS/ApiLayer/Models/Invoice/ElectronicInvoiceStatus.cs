namespace ApiLayer.Models.Invoice
{
    public enum ElectronicInvoiceStatus : byte
    {
        /// <summary>
        /// 處理中
        /// </summary>
        Processing = 1,

        /// <summary>
        /// (開立)成功
        /// </summary>
        Success = 2,

        /// <summary>
        /// (開立)失敗
        /// </summary>
        Fail = 3,

        /// <summary>
        /// 待作廢
        /// </summary>
        PendingVoid = 4,

        /// <summary>
        /// 已作廢
        /// </summary>
        Voided = 5,

        /// <summary>
        /// 待折讓
        /// </summary>
        PendingDiscount = 6,

        /// <summary>
        /// 已折讓
        /// </summary>
        Discounted = 7,
    }
}