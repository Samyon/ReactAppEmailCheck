using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Repository.TaskTbl
{
    public static class Logic
    {
        public static async Task<List<TaskTbl.Entity>> GetTasksExpiredAsync(int secondToExpire)
        {
            var now = DateTime.Now;
            var newTime = now.AddSeconds(-secondToExpire);
            string strTime = newTime.ToString("yyyy-MM-dd HH:mm:ss");
            string where = $"created_at<{strTime}";
            return await TaskTbl.Querys.GetTasksAsync(where);
        }

        //DELETE FROM tasks        WHERE id = 0;



    }
}
