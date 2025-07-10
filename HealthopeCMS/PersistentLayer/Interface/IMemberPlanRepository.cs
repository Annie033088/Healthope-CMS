using DomainLayer.Models;

namespace PersistentLayer.Interface
{
    public interface IMemberPlanRepository
    {
        /// <summary>
        /// 修改會籍狀態
        /// </summary>
        int EditMemberMembershipPlanStatus(MemberMembershipPlan membershipPlan);

        /// <summary>
        /// 修改教練課方案的教練
        /// </summary>
        int EditMemberPersonalTrainingPackageCoach(MemberPersonalTrainingPackage memberPersonalTrainingPackage);
    }
}
