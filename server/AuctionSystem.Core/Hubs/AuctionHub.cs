using Microsoft.AspNetCore.SignalR;

namespace AuctionSystem.Infrastructure.Hubs
{
    public class AuctionHub : Hub
    {
        // לקוחות יכולים "להירשם" לקבלת עדכונים על מכירה ספציפית
        public async Task JoinAuctionGroup(int auctionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Auction_{auctionId}");
        }

        public async Task LeaveAuctionGroup(int auctionId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Auction_{auctionId}");
        }
    }
}