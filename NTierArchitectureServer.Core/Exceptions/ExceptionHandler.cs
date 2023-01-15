using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace NTierArchitectureServer.Core.Exceptions
{
    public class ExceptionHandler : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				context.Response.ContentType= "application/json";

				ErrorResult errorResult = new()
				{
					StatusCode = context.Response.StatusCode,
					Message = ex.Message,
				};

				var json = JsonSerializer.Serialize(errorResult);
				await context.Response.WriteAsync(json);
			}
        }
    }
}
