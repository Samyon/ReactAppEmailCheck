using DbUp;
using DbUp.Engine;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;





namespace DBMigrator
{
    public static class Program
    {
        static void Main()
        {
            SqliteConnection connection = new("Data Source=../../../../ReactApp1.Server\\bin\\Debug\\net9.0\\dbup.db");

            var upgrader =
            DeployChanges.To
            .SqliteDatabase(connection.ConnectionString)
            //.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .WithScriptsFromFileSystem("Migration")
            .LogToConsole()
            .Build();

            var watch = new Stopwatch();
            watch.Start();

            var result = upgrader.PerformUpgrade();

            watch.Stop();
            Display("File", result, watch.Elapsed);


        }

        static void Display(string dbType, DatabaseUpgradeResult result, TimeSpan ts)
        {
            // Display the result
            if (result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success!");
                Console.WriteLine("{0} Database Upgrade Runtime: {1}", dbType,
                    String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));

#if DEBUG
                Console.ReadLine();
#endif   

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.WriteLine("Failed!");

#if DEBUG
                Console.ReadLine();
#endif     
            }
        }
    }
}
