using System.Net;
using System.Text.Json;
using BookingApp.Domain.Exceptions;

namespace BookingApp.WebApi.Middleware
{
    public class ExceptionHandlingMiddleware( RequestDelegate next )
    {
        public async Task InvokeAsync( HttpContext context )
        {
            try
            {
                await next( context );
            }
            catch ( Exception ex )
            {
                await HandleExceptionAsync( context, ex );
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception )
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status400BadRequest,
                NoAvailabilityException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = statusCode;

            var response = new
            {
                error = exception.Message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize( response ) );
        }
    }
}