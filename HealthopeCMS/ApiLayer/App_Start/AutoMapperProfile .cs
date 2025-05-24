using System;
using ApiLayer.Models.Admin.ResponseAdminDto;
using ApiLayer.Models.Coach.Request;
using ApiLayer.Models.Coach.Response;
using ApiLayer.Models.GroupClassSchedule.Response;
using ApiLayer.Models.GroupClassShowcase.Request;
using ApiLayer.Models.GroupClassShowcase.Response;
using ApiLayer.Models.Member.Response;
using AutoMapper;
using DomainLayer.Models;

namespace ApiLayer.App_Start
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // 在這裡配置所有的映射關係
            //CreateMap<RequestAddTaskDto, ToDoTask>();
            //CreateMap<RequestEditTaskDto, ToDoTask>();
            CreateMap<Admin, ResponseGetAdminDto>();
            CreateMap<Member, ResponseGetMemberDto>();
            CreateMap<Member, ResponseGetMemberEditDataByIdDto>();
            CreateMap<Member, ResponseGetMemberDetailDto>();
            CreateMap<Coach, ResponseGetCoachEditDataByIdDto>();
            CreateMap<Coach, ResponseGetCoachDto>();
            CreateMap<RequestAddCoachDto, Coach>()
                .ForMember(dest => dest.ContractStartTime, opt =>
                    opt.MapFrom(src => src.ContractStartTime ?? DateTime.MinValue))
                .ForMember(dest => dest.ContractEndTime, opt =>
                    opt.MapFrom(src => src.ContractEndTime ?? DateTime.MinValue));
            CreateMap<RequestAddShowcaseDto, GroupClassShowcase>();
            CreateMap<GroupClassShowcase, ResponseGetShowcaseDto>();
            CreateMap<GroupClassShowcase, ResponseGetShowcaseDetailDto>();
            CreateMap<GroupClassShowcase, ResponseGetShowcaseEditDataDto>();
            CreateMap<Coach, ScheduleGetCoachDto>();
            CreateMap<GroupClassShowcase, ScheduleGetShowcaseDto>();
            CreateMap<GroupClassSchedule, ResponseGetScheduleDto>();
        }
    }
}