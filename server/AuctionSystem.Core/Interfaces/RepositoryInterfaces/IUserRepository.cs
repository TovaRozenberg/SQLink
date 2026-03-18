using System.Threading.Tasks;
using AuctionSystem.Core.Entities;

namespace AuctionSystem.Core.Interfaces.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task SaveChangesAsync();
    }
}