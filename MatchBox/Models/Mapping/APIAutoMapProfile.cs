using AutoMapper;
using MatchBox.API.Models;
using MatchBox.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Models.Mapping
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

            // AuthenticationController
            CreateMap<DbUserClaim, Claim>().ReverseMap();
            CreateMap<DbUser, RegisterNewUserModel>().ReverseMap();
            CreateMap<DbUser, UpdateUserModel>().ReverseMap();
            CreateMap<System.Security.Claims.Claim, Claim>().ReverseMap();
        }
    }
}
