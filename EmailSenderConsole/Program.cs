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

            string pathToDb = config.GetValue<string>("TaskSettings:PathToDb") ?? "";
            if (pathToDb=="")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Возникло исключение, т. к. невозможно прочитать TaskSettings:PathToDb из appsettings.json");
                Console.ResetColor();
                Console.ReadLine();
                throw new Exception("Возникло исключение, т. к. невозможно прочитать TaskSettings:PathToDb из appsettings.json");
            }

            Db.GetDb.ChangePath(config.GetValue<string>("TaskSettings:PathToDb")!);
            while (true)
            {
                intervalSeconds = config.GetValue<int>("TaskSettings:IntervalSeconds");
                Console.WriteLine($"[{DateTime.Now}] Выполнение задачи");
                try
                {
                    await Logic.MakeAllAsync();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Возникло исключение, текущий набор команд прервался по причине: " + ex.Message + " Скоро будет запущен новый набор");
                    Console.ResetColor();
                    //Logger
                }

                await Task.Delay(TimeSpan.FromSeconds(intervalSeconds));
            }
        }
    }
}