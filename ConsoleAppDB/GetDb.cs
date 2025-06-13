using Db.Repository.TaskTbl.Dtos;
using Microsoft.Data.Sqlite;


namespace Db
{
    public static class GetDb
    {
        //private const string connectionString = "Data Source=bin\\Debug\\net9.0\\dbup.db";
        private const string connectionString = "Data Source=dbup.db";
        public static string GetPath()
        {
            //return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            //string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            //UriBuilder uri = new UriBuilder(codeBase);
            //string path = Uri.UnescapeDataString(uri.Path);
            //return Path.GetDirectoryName(path);

            // Get a Type object.
            //Type t = typeof(GetStr);
            // Instantiate an Assembly class to the assembly housing the Integer type.
            //Assembly assem = Assembly.GetAssembly(t);

            //return assem.Location;
            return "test";
        }

        /// <summary>
        /// Warning! SQL Injection possible
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        [Obsolete("Используйте ExecuteNonQueryParamAsync вместо ExecuteNonQueryAsync.")]
        public static async Task ExecuteNonQueryAsync(string commandText)
        {
            using var connection = new SqliteConnection(connectionString);
            {
                await connection.OpenAsync();
                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = commandText;
                await insertCmd.ExecuteNonQueryAsync();
            }
            Console.WriteLine("Данные успешно добавлены в SQLite.");
            //return "Ok";
        }

        public static async Task ExecuteNonQueryParamAsync(string commandText, TaskInsertDto dto)
        {
            var dictParam = new Dictionary<string, string>();


            //using var connection = new SqliteConnection(connectionString);
            //{
            //    await connection.OpenAsync();


            //    using (var command = new SqliteCommand(commandText, connection))
            //    {
            //        command.Parameters.AddWithValue("@Email", dto.Email);
            //        command.Parameters.AddWithValue("@Code", dto.Code);
            //        command.Parameters.AddWithValue("@IpClient", dto.IpClient);
            //        command.Parameters.AddWithValue("@Session", dto.Session);

            //        await command.ExecuteNonQueryAsync();
            //    }

            //}
            //Console.WriteLine("Данные успешно добавлены в SQLite.");
            //return "Ok";
        }

        public static async Task ExecuteNonQueryParamAsync(string commandText, Dictionary<string, string> dictParam)
        {
            using var connection = new SqliteConnection(connectionString);
            {
                await connection.OpenAsync();
                using (var command = new SqliteCommand(commandText, connection))
                {
                    foreach (var item in dictParam)
                    {
                        command.Parameters.AddWithValue($"@{item.Key}", item.Value);
                    }

                    await command.ExecuteNonQueryAsync();
                }
            }
            Console.WriteLine("Данные успешно добавлены в SQLite.");
            //return "Ok";
        }

        public static async Task<SqliteDataReader?> GetDataReaderAsync(string commandText)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = commandText;
                return await command.ExecuteReaderAsync();                
            }
        }
    }
}
