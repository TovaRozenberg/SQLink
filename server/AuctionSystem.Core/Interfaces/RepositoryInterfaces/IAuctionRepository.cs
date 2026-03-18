using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionSystem.Core.Entities;

namespace AuctionSystem.Core.Interfaces.RepositoryInterfaces
{
    public interface IAuctionRepository
    {
        Task<IEnumerable<Auction>> GetAllActiveAuctionsAsync();
        Task<Auction?> GetAuctionByIdAsync(int id);
        Task AddAuctionAsync(Auction auction);
        Task SaveChangesAsync();
        Task<IEnumerable<Auction>> GetBySellerIdAsync(int sellerId);
    }
}