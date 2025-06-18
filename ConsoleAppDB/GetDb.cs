using Db.Repository.EmailTasks.Dtos;
using Microsoft.Data.Sqlite;


namespace Db
{
    public static class GetDb
    {
        private static object _locker = new object();    
        private static string _connectionString = "Data Source=bin\\Debug\\net9.0\\dbup.db";
        //private static string connectionString = "Data Source=dbup.db";
        public static void ChangePath(string newConnectionString)
        {
            _connectionString = newConnectionString;
        }

        [Obsolete("Warning! SQL Injection possible. Use method with Param")]
        public static async Task ExecuteNonQueryAsync(string commandText)
        {
            using var connection = new SqliteConnection(_connectionString);
            {
                await connection.OpenAsync();
                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = commandText;
                await insertCmd.ExecuteNonQueryAsync();
            }
            Console.WriteLine("Данные успешно добавлены в SQLite.");
            //return "Ok";
        }

        public static async Task ExecuteNonQueryParamAsync(string commandText, EmailTaskInsertDto dto)
        {
            using var connection = new SqliteConnection(_connectionString);
            {
                await connection.OpenAsync();


                using (var command = new SqliteCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Email", dto.Email);
                    command.Parameters.AddWithValue("@Code", dto.Code);
                    command.Parameters.AddWithValue("@IpClient", dto.IpClient);
                    command.Parameters.AddWithValue("@WebSession", dto.WebSession);

                    await command.ExecuteNonQueryAsync();
                }

            }
            //Console.WriteLine("Данные успешно добавлены в SQLite.");
            //return "Ok";
        }

        public static async Task ExecuteNonQueryParamAsync(string commandText, Dictionary<string, string> dictParam)
        {
            using var connection = new SqliteConnection(_connectionString);
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
        }

        public static async Task ExecuteNonQueryParamAsync(string commandText, Dictionary<string, object> dictParam)
        {
            using var connection = new SqliteConnection(_connectionString);
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
        }


        //Можно использовать после закрытия соединения
        public static async Task<List<Dictionary<string, object>>> GetRawQueryResultAsync(string query)
        {
            var result = new List<Dictionary<string, object>>();

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = query;

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var name = reader.GetName(i);
                    var value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    row[name] = value!;
                }

                result.Add(row);
            }

            return result; // 
        }

        //Можно использовать после закрытия соединения
        public static async Task<List<Dictionary<string, object>>> GetRawQueryResultAsync(string query, Dictionary<string, object> dictParam)
        {
            query = Helper.CleanString(query);


            var result = new List<Dictionary<string, object>>();

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = query;
            foreach (var item in dictParam)
            {
                command.Parameters.AddWithValue($"@{item.Key}", item.Value);
            }

            //======================

            string debugSql = command.CommandText;
            foreach (SqliteParameter param in command.Parameters)
            {
                debugSql = debugSql.Replace(param.ParameterName, $"'{param.Value}'");
            }
            Console.WriteLine(debugSql);





            //===================

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var name = reader.GetName(i);
                    var value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    row[name] = value ?? DBNull.Value;
                }

                result.Add(row);
            }

            return result; // 
        }


        public static async Task GetDataReaderAsync(string commandText, Func<SqliteDataReader, Task> handleReaderAsync)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = commandText;
                using (var reader = await command.ExecuteReaderAsync())
                {
                    await handleReaderAsync(reader); // делегируем чтение вызывающему коду
                }              
            }
        }
    }
}
