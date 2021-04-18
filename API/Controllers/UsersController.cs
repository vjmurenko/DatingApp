using System.Collections;
using System.Collections.Generic;
using System.Linq;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly DataContext _dataContext;

		public UsersController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		[HttpGet]
		public ActionResult<IEnumerable<AppUser>> GetUserList()
		{
			return _dataContext.Users.ToList();
		}


		[HttpGet]
		[Route("{id}")]
		public ActionResult<AppUser> GetUser(int id)
		{
			return _dataContext.Users.Find(id);
		}
	}
}