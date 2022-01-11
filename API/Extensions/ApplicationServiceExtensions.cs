using System;
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using API.SinglaR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions {
	public static class ApplicationServiceExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddSingleton<PresenceTracker>();
			services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<IPhotoService, PhotoService>();
			services.AddScoped<IUnityOfWork, UnityOfWork>();
			services.AddScoped<LogUserActivity>();
			services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
			services.AddDbContext<DataContext>(options =>
			{
				var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

				string connStr;
				if (env == "Development")
				{
					// Use connection string from file.
					connStr = config.GetConnectionString("DefaultConnection");
				}
				else
				{
					connStr = config.GetSection("DefaultConnection").Value;
				}

				options.UseNpgsql(connStr);
			});

			return services;
		}
	}
}