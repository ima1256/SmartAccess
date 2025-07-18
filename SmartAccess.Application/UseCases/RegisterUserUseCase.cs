using SmartAccess.Application.DTOs;
using SmartAccess.Domain.Entities;
using SmartAccess.Domain.Repositories;

namespace SmartAccess.Application.UseCases
{
    public class RegisterUserUseCase
    {
        private readonly IUserRepository _repo;

        public RegisterUserUseCase(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task Execute(UserDto dto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                RFIDCard = dto.RFIDCard
            };

            await _repo.AddAsync(user);
        }
    }
}
