using ApiLayer.Models.Admin.RequestAdminDto;
using ApiLayer.Models.Admin.ResponseAdminDto;
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
        }
    }
}