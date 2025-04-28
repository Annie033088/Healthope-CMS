using System;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.AccountAccess.RequestAccountAccessDto;
using ApiLayer.Models.Admin;
using DomainLayer.Interface;
using DomainLayer.Models;
using DomainLayer.Utility;
using PersistentLayer.Interface;

namespace ApiLayer.Service
{
    public class AccountAccessService : IAccountAccessService
    {
        private readonly string adminSessionKey = "AdminSession";
        private readonly IAdminRepository adminRepository;
        private readonly IAppSetting appSetting;
        private readonly IRedisService redisService;
        private readonly ISessionService sessionService;

        public AccountAccessService(IAdminRepository adminRepository, IAppSetting appSetting, IRedisService redisService, ISessionService sessionService)
        {
            this.appSetting = appSetting;
            this.adminRepository = adminRepository;
            this.redisService = redisService;
            this.sessionService = sessionService;
        }

        public (bool success, Admin admin) AdminLogin(RequestLoginDto loginDto)
        {
            try
            {
                string superAdminAccount = appSetting.GetSuperAdminAccount();
                // cookie/redis 過期時間
                int expiryHour = 12;

                // 加密驗證工具
                Hash hash = new Hash();
                string salt;
                // 加密後 pwd
                string pwdHash;

                // 回傳值
                Admin admin;

                // 如果是 sa 登入
                if (loginDto.Account == superAdminAccount)
                {
                    // 取得 config 檔的 hash
                    string superAdminHash = appSetting.GetSuperAdminHash();

                    // 取得鹽值
                    salt = superAdminHash.Substring(0, 36);

                    // 進行加密
                    pwdHash = hash.PwdHash(loginDto.Pwd, salt);

                    // hash 是否相同
                    if (superAdminHash == pwdHash)
                    {
                        AdminIdentity identity = AdminIdentity.SuperAdmin;
                        admin = new Admin() { AdminId = 1, Identity = (byte)identity, Status = true };

                        // asp.net 儲存會話資料
                        sessionService.SaveSession(adminSessionKey, new AdminSession() { AdminId = admin.AdminId, Identity = identity });

                        // redis 儲存登入後的 sessionId 用來判斷後踢前，儲存 ErrorCode 來判斷權限/狀態是否被異動
                        // 過期時間跟 state server 一致為 12 小時
                        TimeSpan expiry = TimeSpan.FromHours(expiryHour);
                        string key = "Admin" + admin.AdminId;
                        AdminRedis adminRedis = new AdminRedis() { SessionId = sessionService.GetSessionId(), ErrorCode = ErrorCodeDefine.Success };
                        redisService.SetValue(key, adminRedis, expiry);
                        return (true, admin);
                    }

                    return (false, null);
                }
                // 其他管理者登入
                else
                {
                    Admin logginInAdmin = adminRepository.GetLoggingInAdmin(loginDto.Account);

                    if (logginInAdmin == null) return (false, null);

                    salt = logginInAdmin.Hash.Substring(0, 36);
                    pwdHash = hash.PwdHash(loginDto.Pwd, salt);

                    if (logginInAdmin.Hash == pwdHash)
                    {
                        admin = new Admin() { AdminId = logginInAdmin.AdminId, Identity = logginInAdmin.Identity, Status = logginInAdmin.Status };
                        sessionService.SaveSession(adminSessionKey, new AdminSession() { AdminId = admin.AdminId, Identity = (AdminIdentity)admin.Identity });

                        // redis 儲存登入後的 sessionId 用來判斷後踢前，儲存 ErrorCode 來判斷權限/狀態是否被異動
                        // 過期時間跟 state server 一致為 12 小時
                        TimeSpan expiry = TimeSpan.FromHours(expiryHour);
                        string key = "Admin" + admin.AdminId;
                        AdminRedis adminRedis = new AdminRedis() { SessionId = sessionService.GetSessionId(), ErrorCode = ErrorCodeDefine.Success };
                        redisService.SetValue(key, adminRedis, expiry);
                        return (true, admin);
                    }

                    return (false, null);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 管理者登出
        /// </summary>
        public bool AdminLogout()
        {
            try
            {
                AdminSession adminSession = sessionService.GetSession<AdminSession>(adminSessionKey);

                string redisKey = "Admin" + adminSession.AdminId;

                // 清除會話
                sessionService.ClearSerssion();

                // 清除 redis 的登入後資料
                redisService.DeleteKey(redisKey);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 管理者是否已登入
        /// </summary>
        public bool AdminLoggedIn()
        {
            try
            {
                AdminSession adminSession = sessionService.GetSession<AdminSession>(adminSessionKey);

                if (adminSession == null) return false;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}