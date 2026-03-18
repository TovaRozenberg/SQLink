using System.Threading.Tasks;
using AuctionSystem.Core.Entities;

namespace AuctionSystem.Core.Interfaces.ServiceInterfaces
{
    public interface IUserService
    {
        Task<User?> RegisterAsync(User user);
        Task<User?> LoginAsync(string email, string password);
    }
}