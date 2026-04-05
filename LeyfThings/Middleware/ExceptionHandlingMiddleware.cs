using LeyfThings.Exceptions;
using LeyfThings.Models;
using System.Text.Json;
using ValidationException = LeyfThings.Exceptions.ValidationException;

namespace LeyfThings.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
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

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var traceId = context.TraceIdentifier;
            var isDev = _env.IsDevelopment();

            var response = ex switch
            {
                NotFoundException notFound => new ErrorResponse
                {
                    Type = "NotFound",
                    Status = StatusCodes.Status404NotFound,
                    Message = notFound.Message,
                    TraceId = traceId,
                    StackTrace = isDev ? notFound.StackTrace : null
                },
                ValidationException validation => new ErrorResponse
                {
                    Type = "ValidationError",
                    Status = StatusCodes.Status400BadRequest,
                    Message = validation.Message,
                    TraceId = traceId,
                    Errors = validation.Errors.Count > 0 ? validation.Errors : null,
                    StackTrace = isDev ? validation.StackTrace : null
                },
                ExternalServiceException external => new ErrorResponse
                {
                    Type = "ServiceUnavailable",
                    Status = StatusCodes.Status503ServiceUnavailable,
                    Message = isDev ? external.Message : $"{external.ServiceName} is currently unavailable. Please try again later.",
                    TraceId = traceId,
                    StackTrace = isDev ? external.StackTrace : null
                },
                _ => new ErrorResponse
                {
                    Type = "InternalServerError",
                    Status = StatusCodes.Status500InternalServerError,
                    Message = isDev ? ex.Message : "An unexpected error occurred.",
                    TraceId = traceId,
                    StackTrace = isDev ? ex.StackTrace : null
                }
            };

            _logger.LogError(ex, "Unhandled exception [{Type}] {Message} | TraceId: {TraceId}", response.Type, ex.Message, traceId);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.Status;

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            await context.Response.WriteAsync(json);
        }
    }
}
