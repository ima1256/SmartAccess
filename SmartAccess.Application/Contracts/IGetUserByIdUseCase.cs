using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartAccess.Application.DTOs;
using SmartAccess.Domain.Entities;

namespace SmartAccess.Application.Contracts
{
    public interface IGetUserByIdUseCase
    {
        public Task<User?> Execute(Guid id);
    }
}