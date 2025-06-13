using System.Collections.Generic;

namespace ApiLayer.Models.PlanTemplate.Response.GetAllType
{
    public class ResponseGetAllTypePlanDto
    {
        /// <summary>
        /// 會籍方案
        /// </summary>
        public List<ResponseGetMembershipPlanDto> MembershipPlanList { get; set; }

        /// <summary>
        /// 私人課方案
        /// </summary>
        public List<ResponseGetPersonalTrainingPackageDto> PersonalTrainingPackageList { get; set; }

        /// <summary>
        /// 票卷方案
        /// </summary>
        public List<ResponseGetTicketPlanDto> TicketPlanList { get; set; }
    }
    public class ResponseGetMembershipPlanDto
    {
        /// <summary>
        /// 會籍方案 Id
        /// </summary>
        public int MembershipPlanId { get; set; }

        /// <summary>
        /// 方案名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 價格
        /// </summary>
        public int Price { get; set; }
    }

    public class ResponseGetPersonalTrainingPackageDto
    {
        /// <summary>
        /// 教練課方案 Id
        /// </summary>
        public int PersonalTrainingPackageId { get; set; }

        /// <summary>
        /// 方案名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 價格
        /// </summary>
        public int Price { get; set; }
    }

    public class ResponseGetTicketPlanDto
    {
        /// <summary>
        /// 一次性票劵 Id 
        /// </summary>
        public int TicketPlanId { get; set; }

        /// <summary>
        /// 價格
        /// </summary>
        public int Price { get; set; }
    }
}