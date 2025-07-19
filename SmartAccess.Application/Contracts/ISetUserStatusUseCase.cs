
namespace SmartAccess.Application.Contracts
{
    public interface ISetUserStatusUseCase
    {
        public Task<bool> Execute(Guid id, bool status);
    }
}
