using API.Extensions;
using API.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseMiddleware<ExceptionMiddleware>();

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
		}
	}
}