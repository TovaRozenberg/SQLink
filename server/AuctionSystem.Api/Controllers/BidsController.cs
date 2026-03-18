using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuctionSystem.Core.Entities;
using AuctionSystem.Core.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using AuctionSystem.Api.DTOs;

namespace AuctionSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidsController : ControllerBase
    {
        private readonly IBidService _bidService;
        private readonly IMapper _mapper;

        public BidsController(IBidService bidService, IMapper mapper)
        {
            _bidService = bidService;
            _mapper = mapper;
        }

        [HttpGet("auction/{auctionId}")]
        public async Task<IActionResult> GetBidsForAuction(int auctionId)
        {
            var bids = await _bidService.GetBidsForAuctionAsync(auctionId);
            var bidDtos = _mapper.Map<IEnumerable<BidDto>>(bids);
            return Ok(bidDtos);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PlaceBid([FromBody] CreateBidDto bid)
        {
            var bidEntity = _mapper.Map<Bid>(bid);
            var success = await _bidService.PlaceBidAsync(bidEntity);

            if (!success)
            {
                return BadRequest("Invalid bid. Auction might be closed or bid amount is too low.");
            }

            var bidDto = _mapper.Map<BidDto>(bid);
            return Ok(bidDto);
        }
    }
}