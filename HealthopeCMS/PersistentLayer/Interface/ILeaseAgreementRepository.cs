using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        (int errorCodeNumber, bool sendEmailFlag) EditLeaseAgreementStatus(LeaseAgreement leaseAgreement);
    }
}
