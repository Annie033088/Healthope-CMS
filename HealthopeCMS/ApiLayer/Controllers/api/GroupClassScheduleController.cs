using System;
using System.Linq;
using System.Web.Http;
using ApiLayer.Filters;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.GroupClassSchedule.Request;
using ApiLayer.Models.GroupClassSchedule.Response;
using DomainLayer.Utility;
using NLog;
using PersistentLayer.Models;

namespace ApiLayer.Controllers.api
{
    [RequestLoggerFilter]
    [VeriyLoginFilter]
    [AdminPermissionAuthFilter]
    public class GroupClassScheduleController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IGroupClassScheduleService groupClassScheduleService;

        public GroupClassScheduleController(IGroupClassScheduleService groupClassScheduleService)
        {
            this.groupClassScheduleService = groupClassScheduleService;
        }


        /// <summary>
        /// 取得 新增團體課程表前 需要的資料
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetShowcaseAndCoach([FromBody] RequestGetShowcaseAndCoachDto getShowcaseAndCoachDto)
        {
            try
            {
                // 驗證前端傳遞的參數是否合法
                ResultResponse response;
                if (getShowcaseAndCoachDto.Category != null &&
                   !Enum.IsDefined(typeof(GroupClassCategory), getShowcaseAndCoachDto.Category))
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                ResponseGetShowcaseAndCoachDto responseGet =
                    groupClassScheduleService.GetShowcaseAndCoach(getShowcaseAndCoachDto);

                if (responseGet == null)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.GetFailed };
                    return Ok(response);
                }

                response = new ResultResponse<ResponseGetShowcaseAndCoachDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = responseGet
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                return Ok(response);
            }
        }

        /// <summary>
        /// 新增團課 schedule
        /// </summary>
        [HttpPost]
        public IHttpActionResult AddSchedule([FromBody] RequestAddScheduleDto addScheduleDto)
        {
            try
            {
                ResultResponse response;
                FormatValidation formatValidation = new FormatValidation();

                // 把時間轉成 UTC Time
                addScheduleDto.Time = addScheduleDto.Time.ToUniversalTime();

                // 設置有效時間參數
                DateTime tomorrowLocal = DateTime.Now.Date.AddDays(1);
                DateTime tomorrowUtc = tomorrowLocal.ToUniversalTime();
                string[] localTimes = new[] { "8:30", "9:40", "10:50", "14:00", "15:10", "16:20", "17:30", "18:40", "19:50", "21:00" };
                DateTime today = DateTime.Today; // 本地時間的今天 00:00
                string[] utcTimes = localTimes.Select(timeStr =>
                {
                    TimeSpan time = TimeSpan.Parse(timeStr);
                    DateTime localDateTime = DateTime.SpecifyKind(today.Add(time), DateTimeKind.Local);
                    DateTime utcDateTime = localDateTime.ToUniversalTime();
                    return utcDateTime.ToString("HH:mm");
                }).ToArray();

                // 格式驗證
                if (!ModelState.IsValid
                    || addScheduleDto.Time.Date < tomorrowUtc.Date
                    || !utcTimes.Contains(addScheduleDto.Time.ToString("HH:mm"))
                    || !formatValidation.ValidInput(true, 1, 20, addScheduleDto.ClassName)
                    || !formatValidation.ValidInput(true, 1, 10, addScheduleDto.Place)
                    || !Enum.IsDefined(typeof(GroupClassCategory), addScheduleDto.Category)
                    || addScheduleDto.Icon < 1 || addScheduleDto.Coach.CoachId < 1
                    || addScheduleDto.MaximumParticipant < 1 || addScheduleDto.MaximumParticipant > 255
                    )
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                response = new ResultResponse() { ErrorCode = groupClassScheduleService.AddSchedule(addScheduleDto) };
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                return Ok(response);
            }
        }

        /// <summary>
        /// 取得團課 schedule
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetSchedule([FromBody] RequestGetGroupClassScheduleDto getScheduleDto)
        {
            try
            {
                FormatValidation formatValidation = new FormatValidation();
                // 驗證前端傳遞的參數是否合法
                bool modelValidFlag = true;

                if (!ModelState.IsValid)
                    modelValidFlag = false;

                // 時間選擇只能存在一個
                if (getScheduleDto.DateRangeFilter != null && getScheduleDto.SpecificDate != null)
                    modelValidFlag = false;
                if (getScheduleDto.DateRangeFilter != null && getScheduleDto.DateRangeFilter != "past"
                    && getScheduleDto.DateRangeFilter != "future" && getScheduleDto.DateRangeFilter != "all")
                    modelValidFlag = false;
                if (!((getScheduleDto.SortOrder == "ascending")
                    || (getScheduleDto.SortOrder == "descending")))
                    modelValidFlag = false;
                if (!((getScheduleDto.SortOption == "time") || (getScheduleDto.SortOption == "reserveParticipant")
                    || (getScheduleDto.SortOption == null)))
                    modelValidFlag = false;
                if (!((getScheduleDto.RecordPerPage == 8) || (getScheduleDto.RecordPerPage == 12)
                    || (getScheduleDto.RecordPerPage == 16)))
                    modelValidFlag = false;
                if (getScheduleDto.Page < 1)
                    modelValidFlag = false;

                ResultResponse response;

                // 格式錯誤
                if (!modelValidFlag)
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

                // 如果有搜尋明確日期的話, 設為 UTC time
                if (getScheduleDto.SpecificDate != null) getScheduleDto.SpecificDate =
                        getScheduleDto.SpecificDate.Value.ToUniversalTime();

                ResponseGetScheduleListDto getScheduleListDto = groupClassScheduleService.GetSchedule(getScheduleDto);
                response = new ResultResponse<ResponseGetScheduleListDto>
                {
                    ErrorCode = ErrorCodeDefine.Success,
                    ApiDataObject = getScheduleListDto
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                return Ok(response);
            }
        }

        // TODO: 取消課程 => 發送信件通知已預約會員
    }
}
