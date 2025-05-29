using System.Collections.Generic;
using ApiLayer.Models.Term.Request;
using ApiLayer.Models.Term.Response;
using PersistentLayer.Models;

namespace ApiLayer.Interface
{
    public interface ITermService
    {
        /// <summary>
        /// 取得舊條款
        /// </summary>
        List<ResponseGetOldTermDto> GetOldTerm(RequestGetOldTermDto getOldTerm);

        /// <summary>
        /// 新增條款
        /// </summary>
        bool AddTerm(RequestAddTermDto addTermDto);

        /// <summary>
        /// 取得條款
        /// </summary>
        ResponseGetTermListDto GetTerm(RequestGetTermDto getTermDto);
    }
}
