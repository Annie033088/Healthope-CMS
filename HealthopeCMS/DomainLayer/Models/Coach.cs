using System;

namespace DomainLayer.Models
{
    public class Coach
    {
        /// <summary>
        /// 教練 ID，主鍵
        /// </summary>
        public int CoachId { get; set; }

        /// <summary>
        /// 登入帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密碼雜湊值
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// 電子郵件地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        public int Phone { get; set; }

        /// <summary>
        /// 教練姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 大頭照圖片網址
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// 簡短自我介紹
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 擅長項目描述
        /// </summary>
        public string Specialty { get; set; }

        /// <summary>
        /// 證照資訊
        /// </summary>
        public string Certification { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 教練類型（自訂分類代碼）
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 合約起始日期
        /// </summary>
        public DateTime ContractStartTime { get; set; }

        /// <summary>
        /// 合約結束日期
        /// </summary>
        public DateTime ContractEndTime { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
