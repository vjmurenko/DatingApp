using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
	public static class IdentityServiceExtensions
	{
		public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddIdentityCore<AppUser>(options =>
				{
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequireDigit = false;
					options.Password.RequireLowercase = false;
					options.Password.RequireUppercase = false;
					options.Password.RequiredLength = 0;
				})
				.AddRoles<AppRole>()
				.AddRoleManager<RoleManager<AppRole>>()
				.AddSignInManager<SignInManager<AppUser>>()
				.AddRoleValidator<RoleValidator<AppRole>>()
				.AddEntityFrameworkStores<DataContext>();

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
						ValidateIssuer = false,
						ValidateAudience = false
					};

					options.Events = new JwtBearerEvents()
					{
						OnMessageReceived = context =>
						{
							var acces_token = context.Request.Query["access_token"];

							var path = context.Request.Path;
							if (!string.IsNullOrEmpty(acces_token) && path.StartsWithSegments("/hubs"))
							{
								context.Token = acces_token;
							}

							return Task.CompletedTask;
						}
					};
				});

			services.AddAuthorization(opt =>
			{
				opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
				opt.AddPolicy("ModeratePhotosRole", policy => policy.RequireRole("Admin", "Moderator"));
			});

			return services;
		}
	}
}