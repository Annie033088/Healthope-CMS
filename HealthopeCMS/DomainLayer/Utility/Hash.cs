using System;
using System.Security.Cryptography;
using System.Text;

namespace DomainLayer.Utility
{
    public class Hash
    {
        private readonly string QrCodeSystemCode = "Healthope-a2053665-81b4-5f49-9f89-9e62e4c8f5cb";

        /// <summary>
        /// 加密 pwd
        /// </summary>
        public string PwdHash(string pwd, string salt)
        {
            try
            {
                byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
                byte[] pwdBytes = Encoding.UTF8.GetBytes(pwd);
                byte[] combinedPwdBytes = new byte[saltBytes.Length + pwdBytes.Length];

                saltBytes.CopyTo(combinedPwdBytes, 0);
                pwdBytes.CopyTo(combinedPwdBytes, saltBytes.Length);

                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(combinedPwdBytes); // { 0xAB, 0xCD, 0x12, ... };

                    // 轉換成 16 進位表示的字串
                    // > "AB-CD-12..." > "ABCD12..." > "abcd12..."
                    string hashHex = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

                    //資料庫以鹽值+加密後pwd儲存
                    return salt + hashHex;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 隨機生成 salt
        /// </summary>
        public string GenerateSalt()
        {
            try
            {
                // 通過 Guid 生成 36 碼 salt
                string salt = Guid.NewGuid().ToString();
                return salt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 加密 qr code 字串
        /// </summary>
        public string QrCodeStringHash(string qrCodeString)
        {
            try
            {
                byte[] saltBytes = Encoding.UTF8.GetBytes(QrCodeSystemCode);
                byte[] pwdBytes = Encoding.UTF8.GetBytes(qrCodeString);
                byte[] combinedPwdBytes = new byte[saltBytes.Length + pwdBytes.Length];

                saltBytes.CopyTo(combinedPwdBytes, 0);
                pwdBytes.CopyTo(combinedPwdBytes, saltBytes.Length);

                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(combinedPwdBytes); // { 0xAB, 0xCD, 0x12, ... };

                    // 轉換成 16 進位表示的字串
                    // > "AB-CD-12..." > "ABCD12..." > "abcd12..."
                    string hashHex = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

                    //資料庫以鹽值+加密後pwd儲存
                    return hashHex;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
