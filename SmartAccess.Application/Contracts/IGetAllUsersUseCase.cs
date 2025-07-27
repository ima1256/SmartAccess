using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartAccess.Domain.Entities;

namespace SmartAccess.Application.Contracts
{
    public interface IGetAllUsersUseCase
    {
        public Task<IEnumerable<User>> Execute();
    }
}