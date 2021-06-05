using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;

namespace API.Data
{
	public interface IUserRepository
	{
		void Update(AppUser user);

		Task<bool> SaveAllAsync();

		Task<IEnumerable<AppUser>> GetUsersAsync();

		Task<AppUser> GetUserById(int id);

		Task<AppUser> GetUserByName(string name);

		Task<IEnumerable<MemberDto>> GetMembersListAsync();

		Task<MemberDto> GetMemberByNameAsync(string name);
	}
}