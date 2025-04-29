using System;
using System.Collections.Generic;
using System.Web.Http;
using ApiLayer.Filters;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Admin.RequestAdminDto;
using ApiLayer.Models.Admin.ResponseAdminDto;
using DomainLayer.Models;
using NLog;

namespace ApiLayer.Controllers.api
{
    [RequestLoggerFilter]
    [AdminKickOutFilter]
    public class AdminController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IAdminService adminService;
        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        /// <summary>
        /// 新增管理者(帳密)
        /// </summary>
        [HttpPost]
        [AdminPermissionAuthFilter]
        public IHttpActionResult AddAdmin(RequestAddAdminDto addAdminDto)
        {
            try
            {
                ResultResponse response;

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
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                return Ok(response);
            }
        }

        /// <summary>
        /// 取得當前登入帳號權限
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetPermission()
        {
            try
            {
                ResultResponse response;

                List<AdminPermission> adminPermissions = adminService.GetPermission();

                if (adminPermissions == null)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.NoPermission };
                    return Ok(response);
                }

                response = new ResultResponse<List<AdminPermission>> { ErrorCode = ErrorCodeDefine.Success, ApiDataObject = adminPermissions };
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                return Ok(response);
            }
        }

        /// <summary>
        /// 取得當前登入帳號權限
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetAdmin(RequestGetAdminDto getAdminDto)
        {
            try
            {
                // 驗證前端傳遞的參數是否合法
                bool modelValidFlag = true;
                if (!((getAdminDto.SearchAccount.Length > 1) || (getAdminDto.SearchAccount == null))) modelValidFlag = false;
                if (!((getAdminDto.SortOrder == "ascending") || (getAdminDto.SortOrder == "descending"))) modelValidFlag = false;
                if (!((getAdminDto.SortOption == "account") || (getAdminDto.SortOption == "status") || (getAdminDto.SortOption == ""))) modelValidFlag = false;
                if (!((getAdminDto.RecordPerPage == 8) || (getAdminDto.RecordPerPage == 12) || (getAdminDto.RecordPerPage == 16))) modelValidFlag = false;

                ResultResponse response;

                // 格式錯誤
                if (!modelValidFlag)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetAdminDto responseGetAdminDto = adminService.GetAdmin(getAdminDto);
                response = new ResultResponse<ResponseGetAdminDto> { ErrorCode = ErrorCodeDefine.Success, ApiDataObject = responseGetAdminDto };
                return Ok(response);
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
