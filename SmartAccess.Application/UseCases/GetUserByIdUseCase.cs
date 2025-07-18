using SmartAccess.Application.DTOs;
using SmartAccess.Domain.Entities;
using SmartAccess.Domain.Repositories;

namespace SmartAccess.Application.UseCases
{
    public class GetUserByIdUseCase
    {
        private readonly IUserRepository _repo;

        public GetUserByIdUseCase(IUserRepository repo) => _repo = repo;

        public async Task<User?> Execute(Guid id) => await _repo.GetByIdAsync(id);
    }


}