using ReactApp1.Server;
using ReactApp1.Server.Middleware;
using Serilog;
public class Program
{
    public static void Main(string[] args)
    {

        Log.Logger = new LoggerConfiguration()
           .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
           .CreateLogger();


        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog(); // заменить стандартный логгер

        builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));

        builder.Services.AddDistributedMemoryCache();

        // 1. Добавляем сессии
        builder.Services.AddSession(options =>
        {
            options.Cookie.Name = ".MyApp.Session";
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();


        var app = builder.Build();

        app.UseGlobalExceptionMiddleware(); //Отлавливаем исключения и превращаем в BadRequest
        app.UseSession();   // добавляем middleware для работы с сессиями



        app.UseDefaultFiles();
        app.MapStaticAssets();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}