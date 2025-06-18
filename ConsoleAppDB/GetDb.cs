using Microsoft.Data.Sqlite;


namespace Db
{
    public static class GetDb
    {
        private static string _connectionString = "Data Source=bin\\Debug\\net9.0\\dbup.db;Cache=Shared;Mode=ReadWriteCreate;Default Timeout=10;";

        public static void ChangePath(string newConnectionString)
        {
            _connectionString = newConnectionString;
        }

        public static async Task ExecuteNonQueryParamAsync(string commandText, Dictionary<string, object> dictParam)
        {
            commandText = Helper.CleanString(commandText);
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
        public static async Task<List<Dictionary<string, object>>> GetRawQueryResultAsync(string query, Dictionary<string, object> dictParam = null)
        {
            query = Helper.CleanString(query);

            var result = new List<Dictionary<string, object>>();

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = query;
            if (dictParam != null)
            {
                foreach (var item in dictParam)
                {
                    command.Parameters.AddWithValue($"@{item.Key}", item.Value);
                }
            }


            //======================  debug
            //string debugSql = command.CommandText;
            //foreach (SqliteParameter param in command.Parameters)
            //{
            //    debugSql = debugSql.Replace(param.ParameterName, $"'{param.Value}'");
            //}
            //Console.WriteLine(debugSql);
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


    }
}
