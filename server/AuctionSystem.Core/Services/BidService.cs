using AuctionSystem.Core.Entities;
using AuctionSystem.Core.Interfaces.ServiceInterfaces;
using AuctionSystem.Core.Interfaces.RepositoryInterfaces;
using Microsoft.AspNetCore.SignalR;
using AuctionSystem.Infrastructure.Hubs; // ודאי שהנתיב נכון ל-Hub שלך
using AutoMapper;

namespace AuctionSystem.Core.Services
{
    public class BidService : IBidService
    {
        private readonly IBidRepository _bidRepository;
        private readonly IAuctionRepository _auctionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<AuctionHub> _hubContext;
        private readonly IMapper _mapper;

        public BidService(
            IBidRepository bidRepository, 
            IAuctionRepository auctionRepository,
            IUserRepository userRepository,
            IHubContext<AuctionHub> hubContext,
            IMapper mapper)
        {
            _bidRepository = bidRepository;
            _auctionRepository = auctionRepository;
            _userRepository = userRepository;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Bid>> GetBidsByAuctionIdAsync(int auctionId)
        {
            return await _bidRepository.GetByAuctionIdAsync(auctionId);
        }

        public async Task<Bid> PlaceBidAsync(int auctionId, int bidderId, decimal amount)
        {
            // 1. שליפת המכירה ובדיקה שהיא קיימת
            var auction = await _auctionRepository.GetByIdAsync(auctionId);
            if (auction == null) throw new Exception("Auction not found.");

            // 2. ולידציה: האם המכירה עדיין פעילה?
            if (auction.EndTime < DateTime.UtcNow) 
                throw new Exception("This auction has already ended.");

            // 3. ולידציה: האם ההצעה גבוהה מהמחיר הנוכחי?
            if (amount <= auction.CurrentPrice)
                throw new Exception("Bid must be higher than the current price.");

            // 4. ולידציה: המוכר לא יכול להציע על המוצר שלו
            if (auction.SellerId == bidderId)
                throw new Exception("You cannot bid on your own auction.");

            // 5. יצירת ההצעה ושמירתה
            var bid = new Bid
            {
                AuctionId = auctionId,
                BidderId = bidderId,
                Amount = amount,
                BidTime = DateTime.UtcNow
            };

            await _bidRepository.AddAsync(bid);

            // 6. עדכון המחיר הנוכחי במכירה (חשוב!)
            auction.CurrentPrice = amount;
            await _auctionRepository.UpdateAsync(auction);

            // --- הקסם של SIGNALR מתחיל כאן ---
            
            // שליפת שם המציע כדי לשלוח לכולם (בשביל ה-UI)
            var bidder = await _userRepository.GetByIdAsync(bidderId);
            
            // שליחת הודעה לכל מי שצופה ב"חדר" של המכירה הזו
            await _hubContext.Clients.Group($"Auction_{auctionId}")
                .SendAsync("ReceiveNewBid", new { 
                    Amount = amount, 
                    BidderName = bidder?.FullName ?? "Anonymous",
                    AuctionId = auctionId
                });

            return bid;
        }
    }
}