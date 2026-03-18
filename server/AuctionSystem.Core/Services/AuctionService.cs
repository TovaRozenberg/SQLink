using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionSystem.Core.Entities;
using AuctionSystem.Core.Interfaces.RepositoryInterfaces;
using AuctionSystem.Core.Interfaces.ServiceInterfaces;

namespace AuctionSystem.Core.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;

        public AuctionService(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        public async Task<IEnumerable<Auction>> GetActiveAuctionsAsync()
        {
            return await _auctionRepository.GetAllActiveAuctionsAsync();
        }

        public async Task<Auction?> GetAuctionByIdAsync(int id)
        {
            return await _auctionRepository.GetAuctionByIdAsync(id);
        }

        public async Task<Auction> CreateAuctionAsync(Auction auction)
        {
            // לוגיקה עסקית: קביעת המחיר הנוכחי כמחיר ההתחלתי, וזמן סיום אם לא הוגדר
            auction.CurrentPrice = auction.StartingPrice;
            if (auction.EndTime == default)
            {
                auction.EndTime = DateTime.UtcNow.AddDays(7); // ברירת מחדל: שבוע למכירה
            }

            await _auctionRepository.AddAuctionAsync(auction);
            await _auctionRepository.SaveChangesAsync();
            return auction;
        }
        // AuctionSystem.Core/Services/AuctionService.cs
        public class AuctionService : IAuctionService
        {
            private readonly IAuctionRepository _auctionRepository;

            public AuctionService(IAuctionRepository auctionRepository)
            {
                _auctionRepository = auctionRepository;
            }

            public async Task<IEnumerable<Auction>> GetAuctionsBySellerIdAsync(int sellerId)
            {
                // כאן אפשר להוסיף לוגיקה עסקית אם צריך (למשל סינון מכירות שנמחקו)
                return await _auctionRepository.GetBySellerIdAsync(sellerId);
            }
        }
    }
}