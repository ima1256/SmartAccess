using SmartAccess.Domain.Repositories;
using SmartAccess.Application.Contracts;

namespace SmartAccess.Application.UseCases
{
    public class SetUserStatusUseCase : ISetUserStatusUseCase
    {
        private readonly IUserRepository _repo;
        public SetUserStatusUseCase(IUserRepository repo) => _repo = repo;

        public async Task<bool> Execute(Guid id, bool status)
        {

            var user = await _repo.GetByIdAsync(id);

            if (user is null) return false;

            user.IsActive = status;

            await _repo.UpdateAsync(user);

            return true;

        }
    }
}