using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
namespace EmailSenderConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Загрузка конфигурации из appsettings.json");
            // Загрузка конфигурации из appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // для доступа к appsettings.json
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            Console.WriteLine("Чтение интервала из конфигурации");
            int intervalSeconds = config.GetValue<int>("TaskSettings:IntervalSeconds");

            bool enabled = config.GetValue<bool>("TaskSettings:LogToConsoleEmptyTasks");

            Console.WriteLine($"Интервал задачи: {intervalSeconds} секунд");

            Console.WriteLine("Цикл запускается. Для выхода нажмите Ctrl+C.");
            Db.GetDb.ChangePath("""Data Source=../../../../ReactApp1.Server\bin\Debug\net9.0\dbup.db""");
            while (true)
            {
                Console.WriteLine($"[{DateTime.Now}] Выполнение задачи");
                await Logic.MakeAllAsync();
                await Task.Delay(TimeSpan.FromSeconds(intervalSeconds));
            }
        }
    }
}