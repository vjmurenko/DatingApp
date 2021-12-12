using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
	[Authorize]
	public class LikesController : BaseApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly ILikesRepository _likesRepository;

		public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
		{
			_userRepository = userRepository;
			_likesRepository = likesRepository;
		}

		[HttpPost("{username}")]
		public async Task<ActionResult> AddUserLike(string username)
		{
			var sourceUser = await _likesRepository.GetUserWithLikes(User.GetUserId());
			var likedUser = await _userRepository.GetUserByName(username);
			string likeMessage;

			if (sourceUser.Username == username)
			{
				return BadRequest("You can't like yourself");
			}

			var userLike = await _likesRepository.GetUserLike(sourceUser.Id, likedUser.Id);

			if (userLike != null)
			{
				sourceUser.LikedUsers.Remove(userLike);
				likeMessage = "You don't like";
			}
			else
			{
				userLike = new UserLike
				{
					LikedUserId = likedUser.Id,
					SourceUserId = sourceUser.Id
				};

				sourceUser.LikedUsers.Add(userLike);
				likeMessage = "You have liked";
			}

			if (await _userRepository.SaveAllAsync())
			{
				return Ok(new JsonResult(likeMessage));
			}

			return BadRequest("Failed to like User");
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery] LikeParams likeParams)
		{
			likeParams.UserId = User.GetUserId();
			var users = await _likesRepository.GetUserLikes(likeParams);
			Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
			return Ok(users);
		}
	}
}