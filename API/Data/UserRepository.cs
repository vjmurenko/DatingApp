﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
	public class UserRepository : IUserRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public UserRepository(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public void Update(AppUser user)
		{
			_context.Entry(user).State = EntityState.Modified;
		}

		public async Task<bool> SaveAllAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<IEnumerable<AppUser>> GetUsersAsync()
		{
			return await _context.Users
				.Include(u => u.Photos)
				.ToListAsync();
		}

		public async Task<AppUser> GetUserByName(string name)
		{
			return await _context.Users
				.Include(u => u.Photos)
				.SingleOrDefaultAsync(u => u.UserName == name);
		}

		public async Task<AppUser> GetUserById(int id)
		{
			return await _context.Users.FindAsync(id);
		}

		public async Task<PagedList<MemberDto>> GetMembersListAsync(UserParams userParams)
		{
			var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
			var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

			var query = _context.Users.OrderBy(u => u.UserName).AsQueryable()
				.Where(u => u.UserName != userParams.CurrentUserName)
				.Where(u => u.Gender == userParams.Gender)
				.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

			query = userParams.OrderBy switch
			{
				"created" => query.OrderByDescending(u => u.Created),
				_ => query.OrderByDescending(u => u.LastActive)
			};
			
			var memberList = query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
				.AsNoTracking();

			return await PagedList<MemberDto>
				.CreateAsync(memberList, userParams.PageNumber, userParams.PageSize);
		}

		public async Task<MemberDto> GetMemberByNameAsync(string name)
		{
			return await _context.Users
				.Where(u => u.UserName == name)
				.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
				.SingleOrDefaultAsync();
		}
	}
}