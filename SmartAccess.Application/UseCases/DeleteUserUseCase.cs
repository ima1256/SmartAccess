using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartAccess.Application.Contracts;
using SmartAccess.Domain.Entities;
using SmartAccess.Domain.Repositories;

namespace SmartAccess.Application.UseCases
{
    public class DeleteUserUseCase : IDeleteUserUseCase
    {
        private readonly IUserRepository _repo;

        public DeleteUserUseCase(IUserRepository repo) => _repo = repo;

        public async Task<DeleteUserResult> Execute(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);

            if (user is null) return DeleteUserResult.NotFound;

            await _repo.DeleteAsync(user);
            return DeleteUserResult.Success;
        }
    }
}