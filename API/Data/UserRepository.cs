using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
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
				.SingleOrDefaultAsync(u => u.Username == name);
		}

		public async Task<AppUser> GetUserById(int id)
		{
			return await _context.Users.FindAsync(id);
		}

		public async Task<IEnumerable<MemberDto>> GetMembersListAsync()
		{
			return await _context.Users
				.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
				.ToListAsync();
		}

		public async Task<MemberDto> GetMemberByNameAsync(string name)
		{
			return await _context.Users
				.Where(u => u.Username == name)
				.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
				.SingleOrDefaultAsync();
		}
	}
}