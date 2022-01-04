using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
	public interface IUserRepository
	{
		void Update(AppUser user);

		Task<IEnumerable<AppUser>> GetUsersAsync();

		Task<AppUser> GetUserById(int id);

		Task<AppUser> GetUserByName(string name);

		Task<PagedList<MemberDto>> GetMembersListAsync(UserParams userParams);

		Task<MemberDto> GetMemberByNameAsync(string name);
		Task<string> GetUserGender(string username);
	}
}