using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Repository.TaskTbl.Dtos
{

    public class TaskInsertDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(150)]
        public string IpClient { get; set; }

        [MaxLength(64)]
        public string Session { get; set; }
        public string Code { get; set; }
    }








}
