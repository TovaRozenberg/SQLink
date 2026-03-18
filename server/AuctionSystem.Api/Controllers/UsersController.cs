using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AuctionSystem.Core.Entities;
using AuctionSystem.Core.Interfaces.ServiceInterfaces;
using AutoMapper;
using AuctionSystem.Api.DTOs;
namespace AuctionSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        // הוספנו את IConfiguration כדי שנוכל לקרוא את המפתח מה-appsettings
        public UsersController(IUserService userService, IConfiguration config, IMapper mapper)
        {
            _userService = userService;
            _config = config;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto user)
        {
            var createdUser = await _userService.RegisterAsync(_mapper.Map<RegisterDto, User>(user));
            if (createdUser == null) return BadRequest("Email is already in use.");

            return Ok(_mapper.Map<UserDto>(createdUser));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {

            var user = await _userService.LoginAsync(loginDto.Email, loginDto.Password);
            if (user == null) return Unauthorized("Invalid email or password.");

            // המשתמש אומת - עכשיו מייצרים לו את הטוקן
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // כאן אנחנו "אורזים" מידע בתוך הטוקן (Claims)
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.FullName)
                }),
                Expires = DateTime.UtcNow.AddDays(7), // תוקף הטוקן
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return Ok(new { Token = jwt , User = _mapper.Map<UserDto>(user)});
        }
    }
}