using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Access.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DbUpdateConcurrencyException)
            {
                context.Response.StatusCode = 409;
                await context.Response.WriteAsJsonAsync(new { message = "A conflict has ocurred, please try again"});
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { message = ex.Message});
            }
            
        }
    }
}