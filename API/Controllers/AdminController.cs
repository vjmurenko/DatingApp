using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
	public class AdminController : BaseApiController
	{
		private readonly UserManager<AppUser> _userManager;

		public AdminController(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		[Authorize(Policy = "RequireAdminRole")]
		[HttpGet("users-with-roles")]
		public async Task<ActionResult> GetUsersWithRoles()
		{
			var users = await _userManager.Users
				.Include(u => u.AppUserRoles)
				.ThenInclude(r => r.AppRole)
				.OrderBy(u => u.UserName)
				.Select(u => new
				{
					Id = u.Id,
					Username = u.UserName,
					Roles = u.AppUserRoles.Select(r => r.AppRole.Name).ToList(),
				}).ToListAsync();

			return Ok(users);
		}

		[Authorize(Policy = "RequireAdminRole")]
		[HttpPost("edit-roles/{username}")]
		public async Task<ActionResult> EditUserRoles(string username, [FromQuery] string roles)
		{
			var user = await _userManager.FindByNameAsync(username);
			if (user == null) return NotFound("Could not found user");

			var selectedRoles = roles.Split(',');
			var userRoles = await _userManager.GetRolesAsync(user);

			var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
			if (!result.Succeeded) return BadRequest("Failed to add roles");

			result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
			if (!result.Succeeded) return BadRequest("Failed to delete roles");

			return Ok(await _userManager.GetRolesAsync(user));
		}

		[Authorize(Policy = "ModeratePhotosRole")]
		[HttpGet("photos-to-moderate")]
		public ActionResult GetPhotosForModeration()
		{
			return Ok("Only admins and moderators can see this");
		}
	}
}