using System;
using System.Collections.Generic;
using System.Web.Http;
using ApiLayer.Interface;
using ApiLayer.Models.Admin;
using ApiLayer.Models.Admin.RequestAdminDto;
using DomainLayer.Interface;
using DomainLayer.Models;
using DomainLayer.Utility;
using PersistentLayer.Interface;

namespace ApiLayer.Service
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository adminRepository;
        private readonly ISessionService sessionService;
        private readonly IAppSetting appSetting;
        private readonly string adminSessionKey = "AdminSession";

        public AdminService(IAdminRepository adminRepository, IAppSetting appSetting, ISessionService sessionService)
        {
            this.adminRepository = adminRepository;
            this.appSetting = appSetting;
            this.sessionService = sessionService;
        }

        /// <summary>
        /// 新增管理者(帳密)
        /// </summary>
        public bool AddAdmin(RequestAddAdminDto addAdminDto)
        {
            try
            {
                // 不能跟超級管理員帳號重複
                if (appSetting.GetSuperAdminAccount() == addAdminDto.Account) return false;

                // 加密驗證工具
                Hash hash = new Hash();
                string salt = hash.GenerateSalt();
                // 加密後 pwd
                string pwdHash = hash.PwdHash(addAdminDto.Pwd, salt);

                bool successFlag;

                // 如果身份存在，就新增管理員
                if (Enum.TryParse<AdminIdentity>(addAdminDto.Identity, out AdminIdentity adminIdentity))
                {
                    Admin admin = new Admin()
                    {
                        Account = addAdminDto.Account,
                        Hash = pwdHash,
                        Identity = (byte)adminIdentity
                    };

                    successFlag = adminRepository.AddAdmin(admin);
                    return successFlag;
                }

                successFlag = false;

                return successFlag;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得當前登入帳號權限
        /// </summary>
        public List<AdminPermission> GetPermission()
        {
            try
            {
                AdminSession adminSession = sessionService.GetSession<AdminSession>(adminSessionKey);
                AdminPermissionUtility adminPermissionUtility = new AdminPermissionUtility();
                return adminPermissionUtility.GetPermissions(adminSession.Identity);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}