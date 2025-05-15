using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayer.Models
{
    public class OperationResult
    {
        /// <summary>
        /// 狀態碼 (參考 enum ErrorCodeDerine)
        /// </summary>
        public int ErrorCodeNumber { get; set; }

        /// <summary>
        /// 例外狀況
        /// </summary>
        public Exception Exception { get; set; } 
    }
}
