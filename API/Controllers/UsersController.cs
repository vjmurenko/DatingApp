using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Authorize]
	public class UsersController : BaseApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public UsersController(IUserRepository userRepository, IMapper mapper)
		{
			_userRepository = userRepository;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserList()
		{
			var users = await _userRepository.GetMembersListAsync();
			return Ok(users);
		}

		[HttpGet("{username}")]
		public async Task<ActionResult<MemberDto>> GetUser(string username)
		{
			var user = await _userRepository.GetMemberByNameAsync(username);
			return user;
		}

		[HttpPut]
		public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
		{
			var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var user =  await _userRepository.GetUserByName(userName);
			_mapper.Map(memberUpdateDto, user);
			
			_userRepository.Update(user);

			if (await _userRepository.SaveAllAsync())
			{
				return NoContent();
			}

			return BadRequest("Failed to update user");
		}
	}
}