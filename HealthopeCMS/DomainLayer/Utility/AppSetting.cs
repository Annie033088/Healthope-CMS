using System;
using System.Configuration;
using DomainLayer.Interface;

namespace DomainLayer.Utility
{
    public class AppSetting : IAppSetting
    {
        /// <summary>
        /// 取得 sa 帳號
        /// </summary>
        public string GetSuperAdminAccount()
        {
            try
            {
                return ConfigurationManager.AppSettings["SuperAdminAccount"];
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得 sa Hash
        /// </summary>
        public string GetSuperAdminHash()
        {
            try
            {
                return ConfigurationManager.AppSettings["SuperAdminHash"];
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得 config 擋 key - value
        /// </summary>
        public string GetConfigurationAppsetting(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
