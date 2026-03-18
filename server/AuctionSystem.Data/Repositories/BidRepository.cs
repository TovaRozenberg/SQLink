using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuctionSystem.Core.Entities;
using AuctionSystem.Core.Interfaces.RepositoryInterfaces;

namespace AuctionSystem.Data.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly AuctionDbContext _context;

        public BidRepository(AuctionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bid>> GetBidsByAuctionIdAsync(int auctionId)
        {
            // שולפים את כל ההצעות למכירה מסוימת, כולל פרטי המציע, ומסדרים מהגבוה לנמוך
            return await _context.Bids
                .Where(b => b.AuctionId == auctionId)
                .Include(b => b.Bidder) 
                .OrderByDescending(b => b.Amount) 
                .ToListAsync();
        }

        public async Task AddBidAsync(Bid bid)
        {
            await _context.Bids.AddAsync(bid);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}