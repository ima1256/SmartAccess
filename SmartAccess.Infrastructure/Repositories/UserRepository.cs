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

        public async Task<IEnumerable<User?>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User?>> SearchAsync(string? text, bool? isEnabled = null)
        {

            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(text))
                query = query.Where(u => u.Username.Contains(text));

            if (isEnabled.HasValue)
                query = query.Where(u => u.IsActive == isEnabled.Value);

            return await query.ToListAsync();
        }

    }
}