using System;
using System.Web.Http;
using ApiLayer.Models;
using ApiLayer.Models.GroupClassShowcase.Request;
using ApiLayer.Models.GroupClassShowcase.Response;
using ApiLayer.Models.Other;
using PersistentLayer.Models;

namespace ApiLayer.Interface
{
    public interface IGroupClassShowcaseService
    {
        /// <summary>
        /// 新增展示用團課
        /// </summary>
        (ErrorCodeDefine errorCode, Exception exception) AddShowcase(RequestAddShowcaseDto addShowcaseDto, FileDto file);

        /// <summary>
        /// 取得展示用課程
        /// </summary>
        ResponseGetShowcaseListDto GetShowcase(RequestGetShowcaseDto getShowcaseDto);

        /// <summary>
        /// 取得展示用課程細項
        /// </summary>
        ResponseGetShowcaseDetailDto GetShowcaseDetail(RequestShowcaseIdDto showcaseIdDto);

        /// <summary>
        /// 取得修改展示用團課頁面的資料
        /// </summary>
        ResponseGetShowcaseEditDataDto GetShowcaseEditDataById(RequestShowcaseIdDto showcaseIdDto);

        /// <summary>
        /// 修改展示用團課
        /// </summary>
        (ErrorCodeDefine errorCode, Exception exception) EditShowcase(RequestEditShowcaseDto editShowcaseDto, FileDto file);

        /// <summary>
        /// 刪除展示用團課
        /// </summary>
        bool DeleteShowcase(RequestShowcaseIdDto showcaseIdDto);
    }
}