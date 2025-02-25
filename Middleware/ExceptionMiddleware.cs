using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace TreeApi.Middleware
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var eventId = Guid.NewGuid().ToString();
            var message = exception.Message;

            if (exception is SecureException)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return response.WriteAsync(JsonSerializer.Serialize(new
                {
                    type = "Secure",
                    id = eventId,
                    data = new { message }
                }));
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return response.WriteAsync(JsonSerializer.Serialize(new
                {
                    type = "Exception",
                    id = eventId,
                    data = new { message = $"Internal server error ID = {eventId}" }
                }));
            }
        }
    }
}