using ConsoleAppDBMigrator;
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
        private static string connectionStr { get; set; } = "Data Source=../../../../ReactApp1.Server\\bin\\Debug\\net9.0\\dbup.db";
        static void Main()
        {
            Migrate migrate = new Migrate(connectionStr);
        }

      
    }
}
