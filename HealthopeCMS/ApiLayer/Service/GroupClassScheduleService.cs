using System;
using System.Collections.Generic;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.GroupClassSchedule.Request;
using ApiLayer.Models.GroupClassSchedule.Response;
using AutoMapper;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace ApiLayer.Service
{
    public class GroupClassScheduleService : IGroupClassScheduleService
    {
        private readonly IMapper mapper;
        private readonly IGroupClassScheduleRepository groupClassScheduleRepository;

        public GroupClassScheduleService(IMapper mapper, IGroupClassScheduleRepository groupClassScheduleRepository)
        {
            this.mapper = mapper;
            this.groupClassScheduleRepository = groupClassScheduleRepository;
        }

        /// <summary>
        /// 取得 新增團體課程表前 需要的資料
        /// </summary>
        public ResponseGetShowcaseAndCoachDto GetShowcaseAndCoach(RequestGetShowcaseAndCoachDto getShowcaseAndCoachDto)
        {
            try
            {
                (List<GroupClassShowcase> showcases, List<Coach> coaches) =
                    groupClassScheduleRepository.GetShowcaseAndCoach(getShowcaseAndCoachDto.Category);

                ResponseGetShowcaseAndCoachDto response = new ResponseGetShowcaseAndCoachDto()
                {
                    CoachList = mapper.Map<List<ScheduleGetCoachDto>>(coaches),
                    ShowcaseList = mapper.Map<List<ScheduleGetShowcaseDto>>(showcases),
                };

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 新增團課 schedule
        /// </summary>
        public ErrorCodeDefine AddSchedule(RequestAddScheduleDto addScheduleDto)
        {
            try
            {
                GroupClassSchedule schedule = new GroupClassSchedule()
                {
                    Category = addScheduleDto.Category,
                    ClassName = addScheduleDto.ClassName,
                    Icon = addScheduleDto.Icon,
                    Time = addScheduleDto.Time,
                    Place = addScheduleDto.Place,
                    MaximumParticipant = addScheduleDto.MaximumParticipant,
                };

                Coach coach = new Coach()
                {
                    CoachId = addScheduleDto.Coach.CoachId,
                    Name = addScheduleDto.Coach.Name,
                    UpdateTime = addScheduleDto.Coach.UpdateTime,
                };

                int errorCodeNumber = groupClassScheduleRepository.AddSchedule(schedule, coach);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumber)) return (ErrorCodeDefine.ServerError);

                ErrorCodeDefine errorCode = (ErrorCodeDefine)errorCodeNumber;
                return errorCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得團課 schedule
        /// </summary>
        public ResponseGetScheduleListDto GetSchedule(RequestGetGroupClassScheduleDto getScheduleDto)
        {
            try
            {
                (List<GroupClassSchedule> schedules, int totalPage) = groupClassScheduleRepository.GetSchedule(getScheduleDto);
                ResponseGetScheduleListDto responseGetScheuleDto = new ResponseGetScheduleListDto()
                {
                    ScheduleList = mapper.Map<List<ResponseGetScheduleDto>>(schedules),
                    TotalPage = totalPage
                };

                return responseGetScheuleDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}