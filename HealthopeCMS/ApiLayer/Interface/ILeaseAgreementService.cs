using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Models.LeaseAgreement.Request;
using System.Web.Http;
using PersistentLayer.Models;
using ApiLayer.Models.LeaseAgreement.Response;
using ApiLayer.Models;

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
    }
}
