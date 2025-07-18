using Microsoft.EntityFrameworkCore;
using SmartAccess.Domain.Entities;
using SmartAccess.Domain.Repositories;
using SmartAccess.Infrastructure.Persistence;

namespace SmartAccess.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SmartAccessDbContext _context;

        public UserRepository(SmartAccessDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}