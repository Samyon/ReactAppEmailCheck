using Db.Repository.EmailTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    }
}
