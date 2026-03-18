using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionSystem.Core.Entities;

namespace AuctionSystem.Core.Interfaces.ServiceInterfaces
{
    public interface IAuctionService
    {
        Task<IEnumerable<Auction>> GetActiveAuctionsAsync();
        Task<Auction?> GetAuctionByIdAsync(int id);
        Task<Auction> CreateAuctionAsync(Auction auction);
        Task<IEnumerable<Auction>> GetAuctionsBySellerIdAsync(int sellerId);
    }
}