using SmartAccess.Domain.Entities;

namespace SmartAccess.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);

        Task AddAsync(User user);

        Task<IEnumerable<User?>> GetUsers();

        Task UpdateAsync(User user);

        Task DeleteAsync(User user);

        Task<IEnumerable<User?>> SearchAsync(string? text, bool? isEnabled = null);



    }
}

