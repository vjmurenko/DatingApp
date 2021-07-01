using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext dataContext, ITokenService tokenService)
        {
            _dataContext = dataContext;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.Username))
            {
                return BadRequest("This username already exist");
            }

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                Username = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _dataContext.Add(user);

            await _dataContext.SaveChangesAsync();

            return new UserDto
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _dataContext.Users
                .Include(u => u.Photos)
                .SingleOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null)
            {
                return Unauthorized("Invalid username");
            }

            var hmac = new HMACSHA512(user.PasswordSalt);

            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Wrong password");
                }
            }

            return new UserDto
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url
            };
        }

        private async Task<bool> UserExist(string username)
        {
            return await _dataContext.Users.AnyAsync(u => u.Username == username.ToLower());
        }
    }
}