using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartAccess.Application.Contracts
{
    public interface IDeleteUserUseCase
    {
        public Task<bool> Execute(Guid id);
    }
}