using System;

namespace AuctionSystem.Api.DTOs
{
    // קבלת הצעת מחיר מהלקוח (את זהות המציע ניקח מהטוקן)
    public class CreateBidDto
    {
        public int AuctionId { get; set; }
        public decimal Amount { get; set; }
    }

    // החזרת הצעת מחיר החוצה (כאן נשתמש בטריק יפה של AutoMapper כדי להביא את שם המציע)
    public class BidDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime BidTime { get; set; }
        public string BidderName { get; set; } = string.Empty; // נשלוף את זה מה-User המקושר!
    }
}