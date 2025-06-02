using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace PersistentLayer.Interface
{
    public interface ILeaseAgreementRepository
    {
        /// <summary>
        /// 新增條款
        /// </summary>
        bool AddLeaseAgreement(LeaseAgreement leaseAgreement);
    }
}
