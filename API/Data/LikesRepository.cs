using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
	public class LikesRepository : ILikesRepository
	{
		private readonly DataContext _context;

		public LikesRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
		{
			return await _context.Likes.FindAsync(sourceUserId, likedUserId);
		}

		public async Task<AppUser> GetUserWithLikes(int userId)
		{
			return await _context.Users
				.Include(u => u.LikedUsers)
				.FirstOrDefaultAsync(u => u.Id == userId);
		}

		public async Task<PagedList<LikeDto>> GetUserLikes(LikeParams likeParams)
		{
			var users = _context.Users.OrderBy(u => u.Username).AsQueryable();
			var likes = _context.Likes.AsQueryable();

			if (likeParams.Predicate == "liked")
			{
				likes = likes.Where(l => l.SourceUserId == likeParams.UserId);
				users = likes.Select(l => l.LikedUser);
			}

			if (likeParams.Predicate == "likedBy")
			{
				likes = likes.Where(l => l.LikedUserId == likeParams.UserId);
				users = likes.Select(l => l.SourceUser);
			}

			var likedUsers = users.Select(u => new LikeDto()
			{
				Id = u.Id,
				KnownAs = u.KnownAs,
				Age = u.DateOfBirth.CalculateAge(),
				City = u.City,
				PhotoUrl = u.Photos.FirstOrDefault(p => p.IsMain).Url,
				Username = u.Username
			});

			return await PagedList<LikeDto>.CreateAsync(likedUsers, likeParams.PageNumber, likeParams.PageSize);
		}
	}
}