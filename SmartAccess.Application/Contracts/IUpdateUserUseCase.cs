using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartAccess.Application.DTOs;
using SmartAccess.Application.UseCases;

namespace SmartAccess.Application.Contracts
{
    public interface IUpdateUserUseCase
    {
        public Task<UpdateUserResponse> Execute(Guid id, UserDto dto);
    }
}