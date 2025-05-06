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
    [VeriyLoginFilter]
    [AdminPermissionAuthFilter]
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
        public IHttpActionResult AddAdmin([FromBody] RequestAddAdminDto addAdminDto)
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
        // TODO: 搜尋的帳號資料 加上截斷前後空隔
        /// <summary>
        /// 取得管理者列表
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetAdmin([FromBody] RequestGetAdminDto getAdminDto)
        {
            try
            {
                // 驗證前端傳遞的參數是否合法
                bool modelValidFlag = true;
                if (!((getAdminDto.SearchAccount == null) || (getAdminDto.SearchAccount.Length > 1))) modelValidFlag = false;
                if (!((getAdminDto.SortOrder == "ascending") || (getAdminDto.SortOrder == "descending"))) modelValidFlag = false;
                if (!((getAdminDto.SortOption == "account") || (getAdminDto.SortOption == "status") || (getAdminDto.SortOption == null))) modelValidFlag = false;
                if (!((getAdminDto.RecordPerPage == 8) || (getAdminDto.RecordPerPage == 12) || (getAdminDto.RecordPerPage == 16))) modelValidFlag = false;
                if (getAdminDto.Page < 1) modelValidFlag = false;

                ResultResponse response;

                // 格式錯誤
                if (!modelValidFlag)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetAdminListDto responseGetAdminListDto = adminService.GetAdmin(getAdminDto);
                response = new ResultResponse<ResponseGetAdminListDto> { ErrorCode = ErrorCodeDefine.Success, ApiDataObject = responseGetAdminListDto };
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
        /// 根據 Id 取得管理者
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetAdminById([FromBody] RequestAdminIdDto getAdminIdDto)
        {
            try
            {
                // 驗證前端傳遞的參數是否合法
                ResultResponse response;

                // 格式錯誤
                if (getAdminIdDto.AdminId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetAdminDto responseGetAdminDto = adminService.GetAdminById(getAdminIdDto);

                // 失敗 (無資料)
                if (responseGetAdminDto == null)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.GetFailed };
                    return Ok(response);
                }

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

        /// <summary>
        /// 修改管理者
        /// </summary>
        [HttpPost]
        public IHttpActionResult EditAdmin([FromBody] RequestEditAdminDto editAdminDto)
        {
            try
            {
                // 驗證前端傳遞的參數是否合法
                ResultResponse response;

                // 未修改過 => 格式錯誤
                if (editAdminDto.Status == null && editAdminDto.Identity == null)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                // 無法轉成現有定義的身份 => 格式錯誤
                if (editAdminDto.Identity != null && !Enum.IsDefined(typeof(AdminIdentity), editAdminDto.Identity))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                bool successFlag = adminService.EditAdmin(editAdminDto);

                // 成功
                if (successFlag)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.Success };
                    return Ok(response);
                }

                // 失敗
                response = new ResultResponse { ErrorCode = ErrorCodeDefine.HasBeenModified };
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
        /// 刪除管理者
        /// </summary>
        [HttpPost]
        public IHttpActionResult DeleteAdmin([FromBody] RequestAdminIdDto adminIdDto)
        {
            try
            {
                // 驗證前端傳遞的參數是否合法
                ResultResponse response;

                if (adminIdDto.AdminId < 1)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                bool successFlag = adminService.DeleteAdmin(adminIdDto);

                // 成功
                if (successFlag)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.Success };
                    return Ok(response);
                }

                // 失敗
                response = new ResultResponse { ErrorCode = ErrorCodeDefine.DeleteFailed };
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
