using AutoMapper;
using AuctionSystem.Core.Entities;
using AuctionSystem.Api.DTOs;

namespace AuctionSystem.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Users
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password)); // אומר למאפר איך למפות שדות עם שמות שונים
            CreateMap<User, UserDto>();
            CreateMap<LoginDto, User>();

            // Auctions
            CreateMap<CreateAuctionDto, Auction>();
            CreateMap<Auction, AuctionDto>();

            // Bids
            CreateMap<CreateBidDto, Bid>();
            CreateMap<Bid, BidDto>()
                // הקסם של AutoMapper: אנחנו שולפים את השם של המציע מתוך טבלת ה-Users המקושרת!
                .ForMember(dest => dest.BidderName, opt => opt.MapFrom(src => src.Bidder.FullName)); 
        }
    }
}