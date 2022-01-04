using System.Threading.Tasks;

namespace API.Interfaces
{
	public interface IUnityOfWork
	{
		public IMessageRepository MessageRepository { get; }
		public IUserRepository UserRepository { get; }
		public ILikesRepository LikesRepository { get; }

		Task<bool> Complete();
		bool HasChanges();
	}
}