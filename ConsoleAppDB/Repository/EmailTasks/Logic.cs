using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Repository.EmailTasks
{
    public static class Logic
    {
        public static async Task<List<Entity>> GetTasksExpiredAsync(int secondToExpire)
        {
            var now = DateTime.UtcNow;
            var newTime = now.AddSeconds(-secondToExpire);
            string strTime = newTime.ToString("yyyy-MM-dd HH:mm:ss");
            string where = $"created_at<'{strTime}'";
            return await Querys.GetTasksAsync(where);
        }

        public static async Task DelTasksExpiredAsync(int secondToExpire)
        {
            var now = DateTime.UtcNow;
            var newTime = now.AddSeconds(-secondToExpire);
            string strTime = newTime.ToString("yyyy-MM-dd HH:mm:ss");
            string where = $"created_at<'{strTime}'";
            await Querys.DeleteTaskAsync(where);
        }




    }
}
