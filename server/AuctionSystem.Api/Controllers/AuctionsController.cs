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
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionService _auctionService;
        private readonly IMapper _mapper;
        public AuctionsController(IAuctionService auctionService, IMapper mapper)
        {
            _auctionService = auctionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveAuctions()
        {
            var auctions = await _auctionService.GetActiveAuctionsAsync();
            var auctionDtos = _mapper.Map<IEnumerable<AuctionDto>>(auctions);
            return Ok(auctionDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuction(int id)
        {
            var auction = await _auctionService.GetAuctionByIdAsync(id);
            if (auction == null) return NotFound();

            var auctionDto = _mapper.Map<AuctionDto>(auction);
            return Ok(auctionDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAuction([FromBody] CreateAuctionDto auction)
        {
            var createdAuction = await _auctionService.CreateAuctionAsync(_mapper.Map<CreateAuctionDto, Auction>(auction));
            var auctionDto = _mapper.Map<AuctionDto>(createdAuction);
            return CreatedAtAction(nameof(GetAuction), new { id = auctionDto.Id }, auctionDto);
        }
        // AuctionSystem.Api/Controllers/AuctionsController.cs
        [HttpGet("my-auctions")]
        [Authorize]
        public async Task<IActionResult> GetMyAuctions()
        {
            // 1. שליפת ה-ID מהטוקן
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId)) return Unauthorized();

            // 2. קריאה לסרוויס (הלוגיקה נמצאת שם!)
            var myAuctions = await _auctionService.GetAuctionsBySellerIdAsync(userId);

            // 3. מיפוי ל-DTO והחזרה ללקוח
            var dtos = _mapper.Map<IEnumerable<AuctionDto>>(myAuctions);
            return Ok(dtos);
        }
    }
}