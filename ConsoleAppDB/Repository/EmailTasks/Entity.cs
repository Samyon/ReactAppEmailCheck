using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Repository.EmailTasks
{
    public class Entity
    {
        public long Id { get; set; }
        public string? Created_at { get; set; }
        public string? Email { get; set; }
        public long Status { get; set; }
        public string? Change_status_at { get; set; }
        public string? Code { get; set; }
        public string? Ip_client { get; set; }
        public string? WebSession { get; set; }
        public long TryCount { get; set; }



    }
}
