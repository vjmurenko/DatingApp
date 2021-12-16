using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
	public class AccountController : BaseApiController
	{
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;

		public AccountController(ITokenService tokenService, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_tokenService = tokenService;
			_mapper = mapper;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpPost("register")]
		public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
		{
			if (await UserExist(registerDto.Username)) return BadRequest("This username already exist");

			var user = _mapper.Map<AppUser>(registerDto);
			user.UserName = registerDto.Username.ToLower();

			var result = await _userManager.CreateAsync(user, registerDto.Password);
			if (!result.Succeeded) return BadRequest(result.Errors);

			var roleResult = await _userManager.AddToRoleAsync(user, "Member");
			if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

			return new UserDto
			{
				Username = user.UserName,
				Token = await _tokenService.CreateToken(user),
				KnownAs = user.KnownAs,
				Gender = user.Gender
			};
		}

		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
		{
			var user = await _userManager.Users
				.Include(u => u.Photos)
				.SingleOrDefaultAsync(u => u.UserName == loginDto.Username.ToLower());

			if (user == null) return Unauthorized("Invalid username");

			var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
			if (!result.Succeeded) return Unauthorized();

			return new UserDto
			{
				Username = user.UserName,
				Token = await _tokenService.CreateToken(user),
				PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url,
				Gender = user.Gender,
				KnownAs = user.KnownAs
			};
		}

		private async Task<bool> UserExist(string username)
		{
			return await _userManager.Users.AnyAsync(u => u.UserName == username.ToLower());
		}
	}
}