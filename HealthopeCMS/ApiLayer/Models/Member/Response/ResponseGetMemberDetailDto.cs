using System;
using System.Collections.Generic;

namespace ApiLayer.Models.Member.Response
{
    public class ResponseGetMemberDetailDto
    {
        /// <summary>
        /// 會員主資料
        /// </summary>
        public ResponseGetMemberDetailMemberDto Member { get; set; }

        /// <summary>
        /// 會員的會籍方案資料
        /// </summary>
        public List<ResponseGetMemberDetailMembershipPlanDto> MemberMembershipPlanList { get; set; }

        /// <summary>
        /// 會員的教練課方案資料
        /// </summary>
        public List<ResponseGetMemberDetailPersonalTrainingPackageDto> MemberPersonalTrainingPackageList { get; set; }

        /// <summary>
        /// 教練資料
        /// </summary>
        public List<ResponseGetMemberDetailCoachDto> CoachList { get; set; }
    }

    public class ResponseGetMemberDetailMemberDto
    {
        /// <summary>
        /// 手機
        /// </summary>
        public int Phone { get; set; }

        /// <summary>
        /// 信箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手機 OTP 是否驗證
        /// </summary>
        public bool PhoneVerified { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 照片路徑
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        public byte Gender { get; set; }

        /// <summary>
        /// 身高
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 體重
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 今年團課缺席次數
        /// </summary>
        public byte AbsenceTime { get; set; }

        /// <summary>
        /// 允許預約團課的時間(在那之前被禁止了)
        /// </summary>
        public DateTime AllowGroupClass { get; set; }

        /// <summary>
        /// 會籍到期日
        /// </summary>
        public DateTime MembershipExpiry { get; set; }

        /// <summary>
        /// 緊急聯絡人姓名
        /// </summary>
        public string EmergencyContactName { get; set; }

        /// <summary>
        /// 緊急聯絡人手機
        /// </summary>
        public int EmergencyContactPhone { get; set; }

        /// <summary>
        /// 緊急聯絡人關係
        /// </summary>
        public string EmergencyContactRelation { get; set; }

        /// <summary>
        /// 創建日
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    public class ResponseGetMemberDetailMembershipPlanDto
    {
        /// <summary>
        /// 會員的會籍方案 Id
        /// </summary>
        public int MemberMembershipPlanId { get; set; }

        /// <summary>
        /// 方案名
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// 時限
        /// </summary>
        public byte Duration { get; set; }

        /// <summary>
        /// 狀態 1:未啟用 ; 2:進行中 ; 3:終止 ; 4:暫停 ; 5:完成
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }

    public class ResponseGetMemberDetailPersonalTrainingPackageDto
    {
        /// <summary>
        /// 會員的教練課方案 Id
        /// </summary>
        public int MemberPersonalTrainingPackageId { get; set; }

        /// <summary>
        /// 教練 Id
        /// </summary>
        public int CoachId { get; set; }

        /// <summary>
        /// 方案名
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// 課堂數
        /// </summary>
        public int SessionCount { get; set; }

        /// <summary>
        /// 狀態 1:進行中 ; 2:終止 ; 3:完成
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }

    public class ResponseGetMemberDetailCoachDto
    {
        /// <summary>
        /// 教練 ID，主鍵
        /// </summary>
        public int CoachId { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        public int Phone { get; set; }

        /// <summary>
        /// 教練姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}