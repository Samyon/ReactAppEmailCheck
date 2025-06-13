using Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime;
using System.Threading.Tasks;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private static readonly Random _random = new();

        public class EmailDto
        {
            //[Required]
            //[EmailAddress]
            //[MaxLength(100)]
            public string Email { get; set; }
        }

        private readonly IConfiguration _configuration;

        public EmailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveEmail([FromBody] EmailDto dto)
        {
            var MaximumNumberOfActiveSessionsOnOneIP = _configuration["Settings:MaximumNumberOfActiveSessionsOnOneIP"];


            Db.Repository.TaskTbl.Dtos.TaskInsertDto insertDto = new Db.Repository.TaskTbl.Dtos.TaskInsertDto();
            insertDto.IpClient = HttpContext.Connection.RemoteIpAddress?.ToString();
            var value = HttpContext.Session.GetString("MyKey");
            insertDto.Session = HttpContext.Session.Id;
            insertDto.Email = dto.Email;
            insertDto.Code = _random.Next(0, 10000).ToString(); // от 0 до 9999 включительно
            
            if (await IsSpamAsync()) return BadRequest(new
            {
                error = "Spam detected",
                details = "С хоста запрещено делать более 5 запросов в минуту, более 10 запросов в 10 минут 20 запросов в течение 4 часов"
            });

            await Db.Repository.TaskTbl.Querys.InsertTaskParamAsync(insertDto);

            return Ok(new { status = "received" });
        }

        private async Task<bool> IsSpamAsync()
        {
            //С хоста запрещено делать более 5 запросов в минуту, более 10 запросов в 10 минут 20 запросов в течение 4 часов
            return false;
        }



    }



}
