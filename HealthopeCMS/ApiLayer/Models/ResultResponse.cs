namespace ApiLayer.Models
{
    public class ResultResponse
    {
        /// <summary>
        /// 錯誤碼
        /// </summary>
        public ErrorCodeDefine ErrorCode { get; set; }
    }

    public class ResultResponse<T> : ResultResponse
    {
        public T ApiDataObject { get; set; }
    }
}