using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiLayer.Interface;
using ApiLayer.Models.Admin.RequestAdminDto;
using ApiLayer.Models;
using NLog;
using Newtonsoft.Json;
using System.IO;
using System.Web;
using ApiLayer.Models.Coach.Request;
using System.Collections.ObjectModel;
using DomainLayer.Utility;

namespace ApiLayer.Controllers.api
{
    public class CoachController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 新增管理者(帳密)
        /// </summary>
        [HttpPost]
        public IHttpActionResult AddAdmin()
        {
            try
            {
                ResultResponse response;
                FormatValidation formatValidation = new FormatValidation();

                // 取得請求的 form data 包括 1.addCoachDto, 2.file
                // 並做格式驗證

                if (!Request.Content.IsMimeMultipartContent())
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                MultipartMemoryStreamProvider provider = new MultipartMemoryStreamProvider();
                Request.Content.ReadAsMultipartAsync(provider).Wait();

                RequestAddCoachDto addCoachDto = null;
                byte[] fileData = null;
                string filename = null;

                foreach (HttpContent content in provider.Contents)
                {
                    string key = content.Headers.ContentDisposition.Name?.Trim('"');
                    bool isFile = content.Headers.ContentDisposition.FileName != null;

                    if (isFile)
                    {
                        filename = content.Headers.ContentDisposition.FileName.Trim('"');
                        fileData = content.ReadAsByteArrayAsync().Result;

                        if (fileData.Length < 1 || !formatValidation.ValidImageFile(fileData, content))
                        {
                            response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                            return Ok(response);
                        }
                    }
                    else
                    {
                        string value = content.ReadAsStringAsync().Result;

                        if (key == "addCoachDto")
                        {
                            addCoachDto = JsonConvert.DeserializeObject<RequestAddCoachDto>(value);
                        }
                    }
                }


                // 帳號密碼不可相同
                if (!ModelState.IsValid || addAdminDto.Account == addAdminDto.Pwd)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                if (adminService.AddAdmin(addAdminDto))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.Success };
                    return Ok(response);
                }
                else
                {
                    response = new ResultResponse() { ErrorCode = ErrorCodeDefine.CreateFailed };
                    return Ok(response);
                }

                //if (fileData != null)
                //{
                //    var savePath = HttpContext.Current.Server.MapPath("~/UploadedFiles/" + filename);
                //    File.WriteAllBytes(savePath, fileData);
                //}
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                return Ok(response);
            }
        }
    }
}
