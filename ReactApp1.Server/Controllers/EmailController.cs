using Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
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

        private readonly IConfiguration _configuration;

        public EmailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveEmail([FromBody] Db.Repository.TaskTbl.Dtos.EmailDto dto)
        {
            var MaximumNumberOfActiveSessionsOnOneIP = _configuration["Settings:MaximumNumberOfActiveSessionsOnOneIP"];

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var value = HttpContext.Session.GetString("MyKey");
            var sessionId = HttpContext.Session.Id;
            Console.WriteLine("Received ip: " + ip);
            Console.WriteLine("Received sessionId: " + sessionId);

            Console.WriteLine("Received email: " + dto.Email);


            dto.Ip_client = ip;
            dto.Session = sessionId;
            //С хоста запрещено делать более 5 запросов в минуту, более 10 запросов в 10 минут 20 запросов в течение 4 часов

            int code = _random.Next(0, 10000); // от 0 до 9999 включительно

            await Db.Repository.TaskTbl.Querys.InsertTaskAsync(dto, code.ToString());

            return Ok(new { status = "received" });
        }





    }
}
