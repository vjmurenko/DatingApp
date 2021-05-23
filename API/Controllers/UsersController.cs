﻿using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
	public class UsersController : BaseApiController
	{
		private readonly DataContext _dataContext;

		public UsersController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult<IEnumerable<AppUser>>> GetUserList()
		{
			var users =  await _dataContext.Users.ToListAsync();
			return users;
		}

		[HttpGet]
		[Authorize]
		[Route("{id}")]
		public async Task<ActionResult<AppUser>> GetUser(int id)
		{
			return await _dataContext.Users.FindAsync(id);
		}
	}
}