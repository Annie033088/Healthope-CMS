namespace ApiLayer.Models.Invoice
{
    public enum PrintInvoiceErrorCode
    {
        /// <summary>
        /// 開立成功
        /// </summary>
        Success = 200,

        /// <summary>
        /// 發票號碼以使用
        /// </summary>
        Used = 4001,

        /// <summary>
        /// 號碼區間不足
        /// </summary>
        InsufficientNumberRange = 5001,

        /// <summary>
        /// 買方統編錯誤
        /// </summary>
        BuyerNumberError = 5003,

        /// <summary>
        /// 系統忙碌
        /// </summary>
        SystemOccupied = 9999,
    }
}