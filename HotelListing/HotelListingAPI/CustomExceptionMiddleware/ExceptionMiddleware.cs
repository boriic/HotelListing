using HotelListingAPI.CustomExceptionMiddleware.CustomExceptions;
using HotelListingAPI.CustomExceptionMiddleware.Models;
using Newtonsoft.Json;
using System.Net;

namespace HotelListingAPI.CustomExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong while processing {context.Request.Path}. Method: {context.Request.Method}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            switch (ex)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case BadHttpRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    break;
                case ArgumentException:
                    statusCode = HttpStatusCode.Conflict;
                    break;
                default:
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorDetails
            {
                StatusCode = (int)statusCode,
                ErrorMessage = ex.Message
            }));
        }
    }
}
