using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Models.LeaseAgreement.Request;
using System.Web.Http;

namespace ApiLayer.Interface
{
    public interface ILeaseAgreementService
    {
        /// <summary>
        /// 新增條款
        /// </summary>
        bool AddLeaseAgreement(RequestAddLeaseAgreementDto addLeaseAgreementDto);
    }
}
