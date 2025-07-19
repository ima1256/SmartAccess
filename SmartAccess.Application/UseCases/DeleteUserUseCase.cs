using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartAccess.Domain.Entities;
using SmartAccess.Domain.Repositories;

namespace SmartAccess.Application.UseCases
{
    public class DeleteUserUseCase
    {
        private readonly IUserRepository _repo;

        public DeleteUserUseCase(IUserRepository repo) => _repo = repo;

        public async Task<bool> Execute(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);

            if (user is null) return false;

            await _repo.DeleteAsync(user);
            return true;
        }
    }
}