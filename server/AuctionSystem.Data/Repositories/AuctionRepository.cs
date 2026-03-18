using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuctionSystem.Core.Entities;
using AuctionSystem.Core.Interfaces.RepositoryInterfaces;

namespace AuctionSystem.Data.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly AuctionDbContext _context;

        public AuctionRepository(AuctionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Auction>> GetAllActiveAuctionsAsync()
        {
            return await _context.Auctions
                .Where(a => a.EndTime > DateTime.UtcNow)
                .Include(a => a.Bids)
                .ToListAsync();
        }

        public async Task<Auction?> GetAuctionByIdAsync(int id)
        {
            return await _context.Auctions
                .Include(a => a.Bids)
                .Include(a => a.Seller)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAuctionAsync(Auction auction)
        {
            await _context.Auctions.AddAsync(auction);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        // AuctionSystem.Infrastructure/Repositories/AuctionRepository.cs
        public async Task<IEnumerable<Auction>> GetBySellerIdAsync(int sellerId)
        {
            return await _context.Auctions
                .Where(a => a.SellerId == sellerId)
                .OrderByDescending(a => a.Id) // שהחדשות יהיו למעלה
                .ToListAsync();
        }
    }
}