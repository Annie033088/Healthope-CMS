using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApiLayer.Models;
using ApiLayer.Models.GroupClassShowcase.Request;
using ApiLayer.Models.Other;

namespace ApiLayer.Interface
{
    public interface IGroupClassShowcaseService
    {
        /// <summary>
        /// 新增展示用團課
        /// </summary>
        (ErrorCodeDefine errorCode, Exception exception) AddShowcase(RequestAddShowcaseDto addShowcaseDto, FileDto file);
    }
}