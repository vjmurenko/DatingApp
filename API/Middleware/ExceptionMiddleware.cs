using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger _logger;
		private readonly IWebHostEnvironment _environment;

		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment environment)
		{
			_next = next;
			_logger = logger;
			_environment = environment;
		}


		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);

				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

				var result = _environment.IsDevelopment()
					? new ApiException(context.Response.StatusCode, e.Message, e.StackTrace?.ToString())
					: new ApiException(context.Response.StatusCode, "Server error");

				var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
				var json = JsonSerializer.Serialize(result, options);

				await context.Response.WriteAsync(json);
			}
		}
	}
}