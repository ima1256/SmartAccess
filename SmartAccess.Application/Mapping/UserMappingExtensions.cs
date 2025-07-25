using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartAccess.Domain.Entities;
using SmartAccess.Application.DTOs;
using AutoMapper;

namespace SmartAccess.Application.Mapping
{
    public static class UserMappingExtensions
    {
        public static UserDto ToDto(this User user, IMapper mapper)
            => mapper.Map<UserDto>(user);

        public static User ToEntity(this UserDto dto, IMapper mapper)
            => mapper.Map<User>(dto);
    }
}