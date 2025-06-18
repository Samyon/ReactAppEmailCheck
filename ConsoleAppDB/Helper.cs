using Db.Repository.EmailTasks;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Db
{
    public static class Helper
    {
        public static string TimeUntil(int dayToExpire = 0, int hourToExpire = 0, int minuteToExpire = 0, int secondToExpire = 0)
        {
            var time = DateTime.UtcNow;
            if (dayToExpire > 0) time = time.AddDays(-dayToExpire);
            if (hourToExpire > 0) time = time.AddHours(-hourToExpire);
            if (minuteToExpire > 0) time = time.AddMinutes(-minuteToExpire);
            if (secondToExpire > 0) time = time.AddSeconds(-secondToExpire);
            return time.ToString("yyyy-MM-dd HH:mm:ss");

        }
        public static string GetCurrentTimeForDB()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string CleanString(string input)
        {
            if (input == null) return string.Empty;

            // Удаляем переносы строк
            string cleaned = Regex.Replace(input, @"\r\n?|\n", " ");

            // Сжимаем все множественные пробелы в один
            cleaned = Regex.Replace(cleaned, @"\s+", " ");

            // Убираем пробелы по краям
            return cleaned.Trim();
        }

        public static string GetDebugSql(DbCommand command)
        {
            string sql = command.CommandText;
            foreach (DbParameter param in command.Parameters)
            {
                string value = param.Value switch
                {
                    null => "NULL",
                    string s => $"'{s.Replace("'", "''")}'",
                    DateTime dt => $"'{dt:yyyy-MM-dd HH:mm:ss}'",
                    _ => param.Value.ToString()
                };

                sql = sql.Replace(param.ParameterName, value);
            }

            return sql;
        }


    }
}
