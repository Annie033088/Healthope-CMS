using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace PersistentLayer.Interface
{
    public interface IMemberPlanRepository
    {
        int EditMemberMembershipPlanStatus(MemberMembershipPlan membershipPlan);
    }
}
