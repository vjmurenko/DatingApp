using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Authorize]
	public class LikesController : BaseApiController
	{
		private readonly IUnityOfWork _unityOfWork;

		public LikesController(IUnityOfWork unityOfWork)
		{
			_unityOfWork = unityOfWork;
		}

		[HttpPost("{username}")]
		public async Task<ActionResult> AddUserLike(string username)
		{
			var sourceUser = await _unityOfWork.LikesRepository.GetUserWithLikes(User.GetUserId());
			var likedUser = await _unityOfWork.UserRepository.GetUserByName(username);
			string likeMessage;

			if (sourceUser.UserName == username)
			{
				return BadRequest("You can't like yourself");
			}

			var userLike = await _unityOfWork.LikesRepository.GetUserLike(sourceUser.Id, likedUser.Id);

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

			if (await _unityOfWork.Complete())
			{
				return Ok(new JsonResult(likeMessage));
			}

			return BadRequest("Failed to like User");
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery] LikeParams likeParams)
		{
			likeParams.UserId = User.GetUserId();
			var users = await _unityOfWork.LikesRepository.GetUserLikes(likeParams);
			Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
			return Ok(users);
		}
	}
}