using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartAccess.Domain.Entities;
using SmartAccess.Domain.Repositories;

namespace SmartAccess.Application.UseCases
{
    public class GetAllUsersUseCase
    {
        private readonly IUserRepository _repo;

        public GetAllUsersUseCase(IUserRepository repo) => _repo = repo;

        public async Task<IEnumerable<User?>> Execute() => await _repo.GetUsers();


    }
}