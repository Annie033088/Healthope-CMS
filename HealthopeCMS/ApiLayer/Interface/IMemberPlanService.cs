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
        ErrorCodeDefine EditMemberMembershipPlanStatus(RequestMemberMembershipPlanStatusDto addInvoiceTrackNumberDto);
    }
}
