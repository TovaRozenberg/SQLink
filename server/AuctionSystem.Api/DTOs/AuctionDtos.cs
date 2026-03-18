using System;

namespace AuctionSystem.Api.DTOs
{
    // ה-DTO שיצרנו קודם ליצירת מכירה
    public class CreateAuctionDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal StartingPrice { get; set; }
        public DateTime EndTime { get; set; }
    }

    // DTO להחזרת נתוני מכירה ללקוח (מונע בעיות של קשרים מעגליים ב-JSON)
    public class AuctionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
        public DateTime EndTime { get; set; }
        public int SellerId { get; set; }
    }
}