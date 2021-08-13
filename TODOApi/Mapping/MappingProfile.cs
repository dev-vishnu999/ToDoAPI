using AutoMapper;
using Repositories;
using Services;
using Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TODOApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ToDoItem, ToDoItemModel>();
            CreateMap<ToDoItemModel, ToDoItem>();

            CreateMap<ToDoUserModel, ToDoUser>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(ur => ur.Email));
        }
    }
}
