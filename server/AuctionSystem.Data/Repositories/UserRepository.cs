using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuctionSystem.Core.Entities;
using AuctionSystem.Core.Interfaces.RepositoryInterfaces;

namespace AuctionSystem.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuctionDbContext _context;

        public UserRepository(AuctionDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            // שליפת משתמש לפי אימייל (שימושי להתחברות/לוגין)
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}