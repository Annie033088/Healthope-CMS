namespace ApiLayer.Models.Term
{
    public enum TermStatus : byte
    {
        /// <summary>
        /// 草稿
        /// </summary>
        Draft = 1,

        /// <summary>
        /// 已發布
        /// </summary>
        Published = 2,

        /// <summary>
        /// 過去發布過的版本
        /// </summary>
        Archived = 3
    }
}