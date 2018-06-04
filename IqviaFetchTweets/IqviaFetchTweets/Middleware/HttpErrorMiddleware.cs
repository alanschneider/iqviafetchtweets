using System;
using System.Net;
using System.Threading.Tasks;
using IqviaFetchTweets.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace IqviaFetchTweets.Middleware
{
    /// <summary>
    /// Customized middleware to send back errors in JSON format.
    /// </summary>
    public class HttpErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            // This is highly simplified. Other types of 4xx exceptions would
            // need to be added to this...
            //
            if (exception is BadRequestException)
                code = HttpStatusCode.BadRequest;

            var result = JsonConvert.SerializeObject(new { code, error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class HttpErrorMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpErrorMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpErrorMiddleware>();
        }
    }
}
