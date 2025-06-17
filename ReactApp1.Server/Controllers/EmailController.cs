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

        [HttpPost("recive_email")]
        public async Task<IActionResult> ReciveEmail([FromBody] EmailDto dto)
        {
            var MaximumNumberOfActiveSessionsOnOneIP = _configuration["Settings:MaximumNumberOfActiveSessionsOnOneIP"];

            var values = new List<Dictionary<string, object>>();
            var value = new Dictionary<string, object>();


            var val = HttpContext.Session.GetString("MyKey");
            //id, created_at, email, status, change_status_at, code, ip_client, web_session, try_count
            value.Add("email", dto.Email);
            value.Add("code", _random.Next(0, 10000).ToString());
            value.Add("ip_client", HttpContext.Connection.RemoteIpAddress?.ToString() ?? "");
            value.Add("web_session", HttpContext.Session.Id);
            value.Add("try_count", 0);

            if (await IsSpamAsync(value)) return BadRequest(new
            {
                error = "Spam detected",
                details = "С хоста запрещено делать более 5 записей в минуту, более 10 записей в 10 минут 20 записей в течение 4 часов"
            });

            string sqlQuery = $@" 
                INSERT INTO email_tasks
                (email,  status, change_status_at, code,       ip_client,   web_session, try_count) VALUES
                (@email, 0,    CURRENT_TIMESTAMP, @code, @ip_client,   @web_session, @try_count);
                        ";


            await GetDb.ExecuteNonQueryParamAsync(sqlQuery, value);

            return Ok(new { status = "save to db" });
        }

        [HttpPost("check_code")]
        public async Task<IActionResult> CheckCode([FromBody] EmailDto dto)
        {

        }

        private async Task<bool> IsSpamAsync(Dictionary<string, object> value)
        {
            //С хоста запрещено делать более 5 записей в минуту, более 10 записей в 10 минут 20 записей в течение 4 часов
            var param = new Dictionary<string, object>
            {
                { "created_at", Db.Helper.TimeUntil(minuteToExpire: 1) },
                { "ip_client", value["ip_client"] }
            };
            string selectStr = $@"SELECT email FROM email_tasks WHERE ip_client='@ip_client' AND created_at<'@created_at'";
            var values = await Db.GetDb.GetRawQueryResultAsync(selectStr, param);
            if (values.Count > 4) return true;


            param["created_at"] = Db.Helper.TimeUntil(minuteToExpire: 10);
            values = await Db.GetDb.GetRawQueryResultAsync(selectStr, param);
            if (values.Count > 9) return true;

            param["created_at"] = Db.Helper.TimeUntil(hourToExpire: 4);
            values = await Db.GetDb.GetRawQueryResultAsync(selectStr, param);
            if (values.Count > 19) return true;

            return false;
        }



    }



}
