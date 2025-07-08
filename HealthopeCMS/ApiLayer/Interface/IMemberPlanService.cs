using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Models;
using ApiLayer.Models.MemberPlan.Request;

namespace ApiLayer.Interface
{
    public interface IMemberPlanService
    {
        /// <summary>
        /// 修改會籍狀態
        /// </summary>
        ErrorCodeDefine EditMemberMembershipPlanStatus(RequestMemberMembershipPlanStatusDto editMemberMembershipPlanStatusDto);

        /// <summary>
        /// 修改教練課方案的教練
        /// </summary>
        ErrorCodeDefine EditMemberPersonalTrainingPackageCoach(RequestEditMemberPersonalTrainingPackageCoachDto editMemberPersonalTrainingPackageCoachDto);
    }
}
