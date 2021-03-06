using System;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
	public class LogUserActivity : IAsyncActionFilter
	{
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var resultContext = await next();
			if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

			var userId = resultContext.HttpContext.User.GetUserId();
			var unityOfWork = resultContext.HttpContext.RequestServices.GetService<IUnityOfWork>();
			var user = await unityOfWork.UserRepository.GetUserById(userId);
			user.LastActive = DateTime.UtcNow;
			await unityOfWork.Complete();
		}
	}
}