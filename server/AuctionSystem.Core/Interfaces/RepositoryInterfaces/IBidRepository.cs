using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionSystem.Core.Entities;

namespace AuctionSystem.Core.Interfaces.RepositoryInterfaces
{
    public interface IBidRepository
    {
        // שליפת כל ההצעות של מכירה ספציפית
        Task<IEnumerable<Bid>> GetBidsByAuctionIdAsync(int auctionId);
        Task AddBidAsync(Bid bid);
        Task SaveChangesAsync();
    }
}