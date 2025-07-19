using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartAccess.Application.DTOs;

namespace SmartAccess.Application.Contracts
{
    public interface IUpdateUserUseCase
    {
        public Task<bool> Execute(Guid id, UserDto dto);
    }
}