using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Repository.TaskTbl
{
    public class Entity
    {
        public int Id { get; set; }
        public string? Created_at { get; set; }
        public string? Email { get; set; }
        public int Status { get; set; }
        public string? Change_status_at { get; set; }
        public string? Code { get; set; }
        public string? Ip_client { get; set; }
        public string? Session { get; set; }



    }
}
