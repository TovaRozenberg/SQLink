using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionSystem.Core.Entities;

namespace AuctionSystem.Core.Interfaces.ServiceInterfaces
{
    public interface IBidService
    {
        Task<IEnumerable<Bid>> GetBidsByAuctionIdAsync(int auctionId);
        Task<Bid> PlaceBidAsync(int auctionId, int bidderId, decimal amount);
    }
}