using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace ReactApp1.Server.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env)
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
                // Всегда логируем подробности
                _logger.LogError(ex, "Необработанное исключение: {Message}", ex.Message);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                object responseObj;

                if (_env.IsDevelopment())
                {
                    // В режиме разработки — подробности
                    responseObj = new
                    {
                        error = ex.Message,
                        stackTrace = ex.StackTrace
                    };
                }
                else
                {
                    // В продакшене — безопасное сообщение + UTC время
                    responseObj = new
                    {
                        error = $"Произошла ошибка {DateTime.UtcNow:u}. Мы работаем над её устранением."
                    };
                }

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                await context.Response.WriteAsync(JsonSerializer.Serialize(responseObj, options));
            }
        }
    }
}
