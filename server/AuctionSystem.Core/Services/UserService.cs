using System.Threading.Tasks;
using AuctionSystem.Core.Entities;
using AuctionSystem.Core.Interfaces.RepositoryInterfaces;
using AuctionSystem.Core.Interfaces.ServiceInterfaces;

namespace AuctionSystem.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> RegisterAsync(User user)
        {
            // בדיקה האם המייל כבר קיים
            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null) return null; // משתמש כבר קיים

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            // במערכת אמיתית כאן נבדוק מול סיסמה מוצפנת (Hash)
            if (user == null || user.PasswordHash != password) return null;
            
            return user;
        }
    }
}