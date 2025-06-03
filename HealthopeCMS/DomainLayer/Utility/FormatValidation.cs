using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 手機末三碼搜尋 格式驗證
        /// </summary>
        public bool ValidSearchPhone(string phone)
        {
            if (phone == null) return true;

            string regex = "^\\d{3}$"; // 3 位數
            return Regex.IsMatch(phone, regex);
        }

        /// <summary>
        /// 驗證有效圖片檔案
        /// </summary>
        public bool ValidImageFile(byte[] fileData, string mimeType)
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

            // 嘗試判斷 MIME 類型
            if (mimeType == null || !allowedMimeTypes.Contains(mimeType)) return false;

            // 根據檔案開頭的 magic number（檔案格式標識）判斷檔案類型
            bool isJPEG = fileData[0] == 0xff && fileData[1] == 0xd8 && fileData[2] == 0xff;
            bool isPNG =
                fileData[0] == 0x89 &&
                fileData[1] == 0x50 &&
                fileData[2] == 0x4e &&
                fileData[3] == 0x47 &&
                fileData[4] == 0x0d &&
                fileData[5] == 0x0a &&
                fileData[6] == 0x1a &&
                fileData[7] == 0x0a;
            bool isWEBP =
                fileData[0] == 0x52 && // "RIFF"
                fileData[1] == 0x49 &&
                fileData[2] == 0x46 &&
                fileData[3] == 0x46 &&
                fileData[8] == 0x57 && // "WEBP"
                fileData[9] == 0x45 &&
                fileData[10] == 0x42 &&
                fileData[11] == 0x50;

            return isJPEG || isPNG || isWEBP;
        }

        /// <summary>
        /// 驗證有效合約日期
        /// </summary>
        public bool ValidContractTime(DateTime? startTime, DateTime? endTime)
        {
            if (startTime == null && endTime == null) return true;
            // 預設值 "0001-01-01" 給過
            else if (startTime == DateTime.MinValue && endTime == DateTime.MinValue) return true;
            else if (startTime == DateTime.MinValue && endTime != DateTime.MinValue) return false;
            else if (startTime != DateTime.MinValue && endTime == DateTime.MinValue) return false;
            else if (startTime > endTime) return false;
            else
            {
                int currentYear = DateTime.UtcNow.Year;
                DateTime minDate = new DateTime(currentYear - 100, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime maxDate = new DateTime(currentYear + 100, 12, 31, 23, 59, 59, DateTimeKind.Utc);

                if (startTime < minDate || startTime > maxDate ||
                    endTime < minDate || endTime > maxDate) return false;
            }

            return true;
        }

        /// <summary>
        /// 驗證教練類別
        /// </summary>
        public bool ValidCoachType(byte type)
        {
            // 私人
            if (type == 1) return true;
            // 約聘
            if (type == 2) return true;

            return false;
        }

        /// <summary>
        /// 驗證通用格式： null / 最短 / 最長 
        /// </summary>
        public bool ValidInput(bool requireNonNull, int? minLength, int? maxLength, string input)
        {
            if (requireNonNull && (input == null)) return false;
            if (minLength != null && input != null && input.Length < minLength) return false;
            if (maxLength != null && input != null && input.Length > maxLength) return false;

            return true;
        }
    }
}
