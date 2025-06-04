using System;
using System.Collections.Generic;
using DomainLayer.Models;
using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface ILeaseAgreementRepository
    {
        /// <summary>
        /// 新增條款
        /// </summary>
        bool AddLeaseAgreement(LeaseAgreement leaseAgreement);

        /// <summary>
        /// 取得條款
        /// </summary>
        (List<LeaseAgreement> leaseAgreements, int totalPage) GetLeaseAgreement(
            RequestGetLeaseAgreementDto getLeaseAgreementDto);

        /// <summary>
        /// 修改租約狀態 (僅限未啟用=>啟用, 啟用=>已完成、取消)
        /// </summary>
        (int errorCodeNumber, bool sendEmailFlag, DateTime leaseEndTime) EditLeaseAgreementStatus(LeaseAgreement leaseAgreement);

        /// <summary>
        /// 修改是否提醒
        /// </summary>
        int EditLeaseAgreementRemind(LeaseAgreement leaseAgreement);

        /// <summary>
        /// 刪除租約(僅限未啟用租約)
        /// </summary>
        bool DeleteLeaseAgreement(int leaseAgreementId);
    }
}
