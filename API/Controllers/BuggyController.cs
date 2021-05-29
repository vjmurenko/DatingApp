using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class BuggyController : BaseApiController
	{
		private readonly DataContext _context;

		public BuggyController(DataContext context)
		{
			_context = context;
		}

		[Authorize]
		[HttpGet("secret")]
		public ActionResult<string> GetSecret()
		{
			return "secret string";
		}

		[HttpGet("notfound")]
		public ActionResult<AppUser> GetNotFound()
		{
			var user = _context.Users.Find(-1);
			if (user == null)
			{
				return NotFound();
			}

			return user;
		}

		[HttpGet("badrequest")]
		public ActionResult GetBadRequest()
		{
			return BadRequest("bad");
		}

		[HttpGet("serverError")]
		public ActionResult<string> ServerError()
		{
			string str = null;
			return str.ToString();
		}
	}
}