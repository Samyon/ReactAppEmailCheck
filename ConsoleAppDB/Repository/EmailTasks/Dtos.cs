using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Repository.EmailTasks.Dtos
{

    public class EmailTaskInsertDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(150)]
        public string IpClient { get; set; }

        [MaxLength(64)]
        public string WebSession { get; set; }
        public string Code { get; set; }
        public long TryCount { get; set; }
    }








}
