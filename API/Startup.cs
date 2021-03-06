using API.Extensions;
using API.Middleware;
using API.SinglaR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API
{
	public class Startup
	{
		private readonly IConfiguration _config;

		public Startup(IConfiguration config)
		{
			_config = config;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddApplicationServices(_config);
			services.AddControllers();
			services.AddCors();
			services.AddIdentityServices(_config);
			services.AddSignalR();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseMiddleware<ExceptionMiddleware>();

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors(x => x.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials()
				.WithOrigins("http://localhost:4200"));

			app.UseAuthentication();
			app.UseAuthorization();
			app.UseDefaultFiles();
			app.UseStaticFiles();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapHub<PresenceHub>("hubs/presence");
				endpoints.MapHub<MessageHub>("hubs/message");
				endpoints.MapFallbackToController("Index", "Fallback");
			});
		}
	}
}