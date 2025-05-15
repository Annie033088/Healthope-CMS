using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Models.Other;

namespace ApiLayer.Interface
{
    public interface IMultipartRequestService<T>
    {
        /// <summary>
        /// 檢查 request content 類型
        /// </summary>
        bool IsMultipartRequest(HttpRequestMessage request);

        /// <summary>
        /// 取得並回傳物件跟檔案
        /// </summary>
        Task<(T dataObject, List<FileDto> files)> GetObjectAndFile(HttpRequestMessage request);
    }
}
