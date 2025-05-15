using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Coach.Request;
using ApiLayer.Models.Other;
using DomainLayer.Utility;
using Newtonsoft.Json;

namespace ApiLayer.Service
{
    public class MultipartRequestService<T> : IMultipartRequestService<T>
    {
        /// <summary>
        /// 檢查 request content 類型
        /// </summary>
        public bool IsMultipartRequest(HttpRequestMessage request)
        {
            try
            {
            return request.Content?.IsMimeMultipartContent() ?? false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得並回傳物件跟檔案
        /// </summary>
        public async Task<(T dataObject, List<FileDto> files)> GetObjectAndFile(HttpRequestMessage request)
        {
            try
            {
                MultipartMemoryStreamProvider provider = await request.Content.ReadAsMultipartAsync(new MultipartMemoryStreamProvider());
                T dataObject = default;
                List<FileDto> files = new List<FileDto>();

                foreach (HttpContent content in provider.Contents)
                {
                    byte[] fileData = null;
                    string fileName = content.Headers.ContentDisposition?.FileName?.Trim('"');
                    string name = content.Headers.ContentDisposition?.Name?.Trim('"');

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        fileData = await content.ReadAsByteArrayAsync();
                        files.Add(new FileDto()
                        {
                            FileData = fileData,
                            MimeType = content.Headers.ContentType?.MediaType,
                        });
                    }
                    else if (name == "dataObject")
                    {
                        string value = await content.ReadAsStringAsync();
                        dataObject = JsonConvert.DeserializeObject<T>(value);
                    }
                }

                return (dataObject, files);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}