using AutoMapper;
using MatchBox.API.Models;
using MatchBox.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Model.Mapping
{
    public class APIAutoMapProfile : Profile
    {
        public APIAutoMapProfile()
            : base()
        {
            CreateMap<User, DbUser>().ReverseMap();
            CreateMap<Group, DbGroup>().ReverseMap(); 
            CreateMap<UserGroup, DbUserGroup>().ReverseMap();

            CreateMap<Event, DbEvent>().ReverseMap();
        }
    }
}
