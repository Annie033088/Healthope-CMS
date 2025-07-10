namespace ApiLayer.Models.Invoice
{
    public enum ElectronicInvoiceCategory : byte
    {
        /// <summary>
        /// 主發票
        /// </summary>
        Main = 1,

        /// <summary>
        /// 違約金發票
        /// </summary>
        Penalty = 2,
    }
}