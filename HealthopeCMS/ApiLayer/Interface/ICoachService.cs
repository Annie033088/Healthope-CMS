using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Models;
using ApiLayer.Models.Coach.Request;
using ApiLayer.Models.Coach.Response;
using ApiLayer.Models.Other;
using PersistentLayer.Models;

namespace ApiLayer.Interface
{
    public interface ICoachService
    {
        /// <summary>
        /// 新增教練
        /// </summary>
        (ErrorCodeDefine errorCode, Exception exception) AddCoach(RequestAddCoachDto addCoachDto, FileDto file);

        /// <summary>
        /// 取得教練清單
        /// </summary>
        ResponseGetCoachListDto GetCoach(RequestGetCoachDto getCoachDto);

        /// <summary>
        /// 取得修改教練頁面的資料
        /// </summary>
        ResponseGetCoachEditDataByIdDto GetCoachEditDataById(RequestCoachIdDto coachIdDto);
    }
}
