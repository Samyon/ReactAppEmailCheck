using Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailController> _logger;

        public class EmailDto
        {
            [Required]
            [EmailAddress]
            [MaxLength(100)]
            public string Email { get; set; }
        }

        public EmailController(IConfiguration configuration, ILogger<EmailController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        private async Task<bool> IsSpamAsync(Dictionary<string, object> value)
        {
            //С хоста запрещено делать более 5 записей в минуту, более 10 записей в 10 минут 20 записей в течение 4 часов
            var param = new Dictionary<string, object>
            {
                { "created_at", Db.Helper.TimeUntil(minuteToExpire: 1) },
                { "ip_client", value["ip_client"] }
            };
            string selectStr = $@"SELECT email FROM email_tasks WHERE ip_client=@ip_client AND created_at>@created_at";
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



        [HttpPost("recive_email")]
        public async Task<IActionResult> ReciveEmail([FromBody] EmailDto dto)
        {
            var value = new Dictionary<string, object>();
            HttpContext.Session.SetString("MyKey", "MyKey2");
            var val = HttpContext.Session.GetString("MyKey");//activate session
            value.Add("email", dto.Email);
            value.Add("code", _random.Next(0, 10000).ToString());
            value.Add("ip_client", HttpContext.Connection.RemoteIpAddress?.ToString() ?? "");
            value.Add("web_session", HttpContext.Session.Id);
            value.Add("try_count", 0);

            if (_configuration.GetValue<bool>("MySettings:UseAntiSpam") && await IsSpamAsync(value))
            {
                _logger.LogWarning("Spam detected", DateTime.UtcNow);
                return BadRequest(new
                {
                    error = "Spam detected",
                    details = "С хоста запрещено делать более 5 записей в минуту, более 10 записей в 10 минут 20 записей в течение 4 часов"
                });
            }


            string sqlQuery = $@" INSERT INTO email_tasks
                (email,  status, change_status_at, code,       ip_client,   web_session, try_count) VALUES
                (@email, 0,    CURRENT_TIMESTAMP, @code, @ip_client,   @web_session, @try_count);     ";

            await GetDb.ExecuteNonQueryParamAsync(sqlQuery, value);
            _logger.LogInformation("Email - {dto.Email} внесён в БД, ждём подтверждения", DateTime.UtcNow);
            return Ok(new { status = "save to db" });
        }

        public class CodeDto
        {
            [Required]
            [MaxLength(100)]
            public string Code { get; set; }
        }

        [HttpPost("check_code")]
        public async Task<IActionResult> CheckCode([FromBody] CodeDto dto)
        {
            var param = new Dictionary<string, object>();
            HttpContext.Session.SetString("MyKey", "MyKey2");
            var val = HttpContext.Session.GetString("MyKey");//activate session
            param.Add("ip_client", HttpContext.Connection.RemoteIpAddress?.ToString() ?? "");
            param.Add("web_session", HttpContext.Session.Id);
            string sqlStr = $@"SELECT id, code, try_count, email FROM email_tasks WHERE 
                                      web_session=@web_session AND 
                                      ip_client=@ip_client AND 
                                      try_count<3 ORDER BY id DESC LIMIT 1;";
            var values = await Db.GetDb.GetRawQueryResultAsync(sqlStr, param);
            if (values.Count == 0) return BadRequest(new
            {
                error = "Error confirmation",
                details = "В системе нет данных, что вы запрашивали подтверждение для какой-либо почты"
            });


            long try_count = (long)values[0]["try_count"];
            try_count++;

            if (_configuration.GetValue<bool>("MySettings:UseAntiSpam") && try_count > 3) return BadRequest(new
            {
                error = "Spam detected",
                details = "Вы ввели неправильный код более 3 раз, теперь вам придётся заново запросить код для этой почты"
            });


            param = new Dictionary<string, object>();
            param.Add("try_count", try_count);
            param.Add("id", values[0]["id"]);

            sqlStr = $@" UPDATE email_tasks  SET try_count=@try_count WHERE id=@id;";
            await Db.GetDb.ExecuteNonQueryParamAsync(sqlStr, param);

            if (dto.Code != values[0]["code"].ToString()) return BadRequest(new
            {
                error = "Неверный код",
                details = "Неверный код"
            });

            //Вставляем подтверждённый email
            param = new Dictionary<string, object>();
            param.Add("email", values[0]["email"]);

            sqlStr = $@" INSERT INTO confirmed_emails (email) VALUES(@email);";
            await Db.GetDb.ExecuteNonQueryParamAsync(sqlStr, param);

            //Удаляем подтверждённый email
            param = new Dictionary<string, object>();
            param.Add("id", values[0]["id"]);

            sqlStr = $@" DELETE FROM email_tasks WHERE id=@id;";
            await Db.GetDb.ExecuteNonQueryParamAsync(sqlStr, param);
            _logger.LogInformation($"Для Email - {{{values[0]["email"]}}}  код подтверждён", DateTime.UtcNow);
            return Ok(new { status = "code confirm" });
        }






    }



}
