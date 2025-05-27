using System;

namespace PersistentLayer.Models
{
    public class ResultWithException
    {
        /// <summary>
        /// 狀態碼 (參考 enum ErrorCodeDefine)
        /// </summary>
        public int ErrorCodeNumber { get; set; }

        /// <summary>
        /// 例外狀況
        /// </summary>
        public Exception Exception { get; set; }
    }
}
