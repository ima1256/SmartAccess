using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartAccess.Application.DTOs;
using SmartAccess.Domain.Entities;

namespace SmartAccess.Application.Mapping
{
    public class UserProfile : Profile
    {

        public UserProfile()
        {
            CreateMap<User, UserDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)));
        }
    }
}