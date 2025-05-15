using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Models;
using ApiLayer.Models.Coach.Request;
using ApiLayer.Models.Other;

namespace ApiLayer.Interface
{
    public interface ICoachService
    {
        /// <summary>
        /// 新增教練
        /// </summary>
        (ErrorCodeDefine errorCode, Exception exception) AddCoach(RequestAddCoachDto addCoachDto, FileDto file);
    }
}
