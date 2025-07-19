using SmartAccess.Application.DTOs;

namespace SmartAccess.Application.Contracts
{
    public interface IRegisterUserUseCase
    {
        public Task Execute(UserDto dto);
    }
}