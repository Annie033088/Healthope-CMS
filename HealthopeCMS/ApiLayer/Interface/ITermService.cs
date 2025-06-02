using System.Collections.Generic;
using System.Web.Http;
using ApiLayer.Models;
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

        /// <summary>
        /// 取得修改條款頁面的資料
        /// </summary>
        ResponseGetTermEditDataByIdDto GetTermEditDataById(RequestTermIdDto termIdDto);

        /// <summary>
        /// 修改條款
        /// </summary>
        ErrorCodeDefine EditTerm(RequestEditTermDto editTermDto);

        /// <summary>
        /// 修改條款狀態 (僅限草稿=>發布)
        /// </summary>
        ErrorCodeDefine EditTermStatus(RequestEditTermStatusDto editTermStatusDto);

        /// <summary>
        /// 取得條款的詳細資料
        /// </summary>
        ResponseGetTermDetailDto GetTermDetail(RequestTermIdDto termIdDto);

        /// <summary>
        /// 刪除條款
        /// </summary>
        bool DeleteTerm([FromBody] RequestTermIdDto termIdDto);
    }
}
