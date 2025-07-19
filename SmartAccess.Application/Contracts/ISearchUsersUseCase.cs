using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartAccess.Application.DTOs;
using SmartAccess.Domain.Entities;

namespace SmartAccess.Application.Contracts
{
    public interface ISearchUsersUseCase
    {
        public Task<IEnumerable<UserDto?>> Execute(string? text, bool? isEnabled);
    }
}