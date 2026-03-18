using System.Collections.Generic;

namespace AuctionSystem.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public ICollection<Auction> Auctions { get; set; } = new List<Auction>();
        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
    }
}