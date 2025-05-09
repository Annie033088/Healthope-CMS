using ApiLayer.Models.Admin.ResponseAdminDto;
using ApiLayer.Models.Member;
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
        }
    }
}