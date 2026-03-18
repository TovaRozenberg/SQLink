using System;
using System.Collections.Generic;

namespace AuctionSystem.Core.Entities
{
    public class Auction
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal StartingPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public DateTime EndTime { get; set; }
        public int SellerId { get; set; }
        public User Seller { get; set; } = null!;
        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
    }
}