﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Http;
using ApiLayer.Filters;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.AccountAccess.RequestAccountAccessDto;
using DomainLayer.Models;
using DomainLayer.Utility;
using NLog;

namespace ApiLayer.Controllers.api
{
    [RequestLoggerFilter]
    public class AccountAccessController : ApiController
    {
        private readonly AdminPermissionUtility adminPermissionUtility = new AdminPermissionUtility();
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IAccountAccessService accountAccessService;

        public AccountAccessController(IAccountAccessService accountAccessService)
        {
            this.accountAccessService = accountAccessService;
        }

        /// <summary>
        /// 管理者登入
        /// </summary>
        [HttpPost]
        public IHttpActionResult VerifyAdminLogin([FromBody] RequestLoginDto loginDto)
        {
            try
            {
                ResultResponse response;

                if (!ModelState.IsValid || loginDto.Account == loginDto.Pwd)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                (bool success, Admin admin) = accountAccessService.AdminLogin(loginDto);

                if (!success)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.LoginFailed };
                    return Ok(response);
                }

                if (admin.Status == false)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.Baned };
                    return Ok(response);
                }

                response = new ResultResponse<List<AdminPermission>>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = adminPermissionUtility.GetPermissions((AdminIdentity)admin.Identity)
                };
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
        /// 管理者登出
        /// </summary>
        [HttpPost]
        [VeriyLoginFilter]
        public IHttpActionResult AdminLogout()
        {
            try
            {
                if (accountAccessService.AdminLogout())
                {
                    ResultResponse response = new ResultResponse { ErrorCode = ErrorCodeDefine.Success };
                    return Ok(response);
                }
                else
                {
                    ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
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
        /// 管理者是否有權限 / 是否已登入
        /// </summary>
        [HttpPost]
        public IHttpActionResult HavePermission([FromBody] List<RequestPermissionDto> havePermissionDto)
        {
            try
            {
                return Json(new ResultResponse() { ErrorCode = accountAccessService.AdminHavePermission(havePermissionDto) });
            }
            catch (Exception)
            {
                ResultResponse responseDto;
                responseDto = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                return Json(responseDto);
            }
        }

        /// <summary>
        /// 管理者修改自己的密碼
        /// </summary>
        [HttpPost]
        [VeriyLoginFilter]
        public IHttpActionResult EditSelfPwd([FromBody] RequestEditSelfPwdDto editSelfPwdDto)
        {
            try
            {
                ResultResponse response;
                // 格式需為 8~20 位英文數字
                string pattern = @"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,20}$";

                if (!Regex.IsMatch(editSelfPwdDto.OldPwd, pattern) || !Regex.IsMatch(editSelfPwdDto.NewPwd, pattern))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                response = new ResultResponse { ErrorCode = accountAccessService.EditSelfPwd(editSelfPwdDto) };
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
