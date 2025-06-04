using System.Web.Http;
using ApiLayer.Models;
using ApiLayer.Models.LeaseAgreement.Request;
using ApiLayer.Models.LeaseAgreement.Response;
using PersistentLayer.Models;

namespace ApiLayer.Interface
{
    public interface ILeaseAgreementService
    {
        /// <summary>
        /// 新增條款
        /// </summary>
        bool AddLeaseAgreement(RequestAddLeaseAgreementDto addLeaseAgreementDto);

        /// <summary>
        /// 取得條款
        /// </summary>
        ResponseGetLeaseAgreementListDto GetLeaseAgreement(RequestGetLeaseAgreementDto getLeaseAgreementDto);

        /// <summary>
        /// 修改租約狀態 (僅限未啟用=>啟用, 啟用=>已完成、取消)
        /// </summary>
        ErrorCodeDefine EditLeaseAgreementStatus(RequestEditLeaseAgreementStatusDto editLeaseAgreementStatusDto);

        /// <summary>
        /// 修改是否提醒
        /// </summary>
        ErrorCodeDefine EditLeaseAgreementRemind(RequestEditLeaseAgreementRemindDto editLeaseAgreementRemindDto);

        /// <summary>
        /// 刪除租約(僅限未啟用租約)
        /// </summary>
        bool DeleteLeaseAgreement(RequestLeaseAgreementIdDto leaseAgreementIdDto);
    }
}
