using System;
using System.Collections.Generic;
using ApiLayer.Interface;
using ApiLayer.Models.Admin;
using ApiLayer.Models.Admin.RequestAdminDto;
using ApiLayer.Models.Admin.ResponseAdminDto;
using AutoMapper;
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
        private readonly IMapper mapper;

        public AdminService(IAdminRepository adminRepository, IAppSetting appSetting, ISessionService sessionService,
          IMapper mapper)
        {
            this.adminRepository = adminRepository;
            this.appSetting = appSetting;
            this.sessionService = sessionService;
            this.mapper = mapper;
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

        /// <summary>
        /// 取得管理者清單(列表)
        /// </summary>
        public ResponseGetAdminListDto GetAdmin(RequestGetAdminDto getAdminDto)
        {
            try
            {
                (List<Admin> admins, int totalPage) = adminRepository.GetAdmin(getAdminDto);
                ResponseGetAdminListDto responseGetAdminDto = new ResponseGetAdminListDto()
                {
                    AdminList = mapper.Map<List<ResponseGetAdminDto>>(admins),
                    TotalPage = totalPage
                };

                return responseGetAdminDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根據 Id 取得管理者
        /// </summary>
        public ResponseGetAdminDto GetAdminById(RequestAdminIdDto getAdminIdDto)
        {
            try
            {
                Admin admin = adminRepository.GetAdminById(getAdminIdDto.AdminId);
                if (admin == null) return null;

                ResponseGetAdminDto responseGetAdminDto = mapper.Map<ResponseGetAdminDto>(admin);
                return responseGetAdminDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改管理者
        /// </summary>
        public bool EditAdmin(RequestEditAdminDto editAdminDto)
        {
            try
            {
                return adminRepository.EditAdmin(editAdminDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 刪除管理者
        /// </summary>
        public bool DeleteAdmin(RequestAdminIdDto adminIdDto)
        {
            try
            {
                return adminRepository.DeleteAdmin(adminIdDto.AdminId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}