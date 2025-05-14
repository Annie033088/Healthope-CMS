using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace DomainLayer.Utility
{
    public class FormatValidation
    {
        /// <summary>
        /// 驗證帳號
        /// </summary>
        public bool ValidAccount(string account)
        {
            // 8~20 位英文數字
            string AccountRegex = "^(?=.*[a-zA-Z])(?=.*\\d)[a-zA-Z\\d]{8,20}$";
            return Regex.IsMatch(account, AccountRegex);
        }

        /// <summary>
        /// 驗證密碼
        /// </summary>
        public bool ValidPwd(string pwd)
        {
            // 8~20 位英文數字
            string PwdRegex = "^(?=.*[a-zA-Z])(?=.*\\d)[a-zA-Z\\d]{8,20}$";
            return Regex.IsMatch(pwd, PwdRegex);
        }

        /// <summary>
        /// 驗證信箱
        /// </summary>
        public bool ValidEmail(string email)
        {
            // 可空
            if (String.IsNullOrEmpty(email)) return true;

            // [^\s@] 代表至少一個不是空白或 @ 的字元
            string emailRegex = "^[^\\s@]+@[^\\s@]+\\.[^\\s@]+$"; // EX: abc@ewq.ee
            if (email.Length > 254) return false; // 規定總長最長 254

            string[] parts = email.Split('@');
            if (parts.Length != 2) return false;

            string localPart = parts[0];
            string domain = parts[1];

            if (
              localPart.Length < 3 || // 建議最少 3 字元
              localPart.Length > 64 || // 規定 @以前 最長 64
              domain.Length > 251 // 不得超過 254 - 3
            )
            {
                return false;
            }

            return Regex.IsMatch(email, emailRegex);
        }

        /// <summary>
        /// 驗證有效手機號碼
        /// </summary>
        public bool ValidPhone(int phone)
        {
            string phoneRegex = "^9\\d{8}$"; // 9 開頭, 加後 8 位數
            return Regex.IsMatch(phone.ToString(), phoneRegex);
        }

        public bool ValidImageFile(byte[] fileData, HttpContent fileContent)
        {
            // 設定允許的最大檔案大小（以 byte 為單位）
            const int maxFileSizeByte = 2 * 1024 * 1024; // 2MB

            // 允許的 MIME 類型
            List<string> allowedMimeTypes = new List<string>
            {
                "image/jpeg",
                "image/png",
                "image/webp"
            };

            // 驗證檔案是否存在
            if (fileData == null || fileData.Length < 0) return false;
            
            // 檢查檔案大小
            if (fileData.Length > maxFileSizeByte) return false;

            // 嘗試判斷 MIME 類型（需透過副檔名或 magic number）
            // 這裡簡單示範：透過 Content-Type header（如果有）檢查
            string fileMimeType = fileContent.Headers.ContentType?.MediaType;

            if (fileMimeType == null || !allowedMimeTypes.Contains(fileMimeType)) return false;

            return true;
        }
    }
}
