using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartAccess.Application.UseCases;

namespace SmartAccess.Application.Contracts
{
    public interface IDeleteUserUseCase
    {
        public Task<DeleteUserResult> Execute(Guid id);
    }
}