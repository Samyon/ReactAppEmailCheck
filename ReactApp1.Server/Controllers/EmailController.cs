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
        private readonly IConfiguration _configuration;

        public class EmailDto
        {
            [Required]
            [EmailAddress]
            [MaxLength(100)]
            public string Email { get; set; }
        }



        private async Task<bool> IsSpamAsync(Dictionary<string, object> value)
        {
            //С хоста запрещено делать более 5 записей в минуту, более 10 записей в 10 минут 20 записей в течение 4 часов
            var param = new Dictionary<string, object>
            {
                { "created_at", Db.Helper.TimeUntil(minuteToExpire: 1) },
                { "ip_client", value["ip_client"] }
            };
            string selectStr = $@"SELECT email FROM email_tasks WHERE ip_client=@ip_client AND created_at<@created_at";
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
                (@email, 0,    CURRENT_TIMESTAMP, @code, @ip_client,   @web_session, @try_count);     ";

            await GetDb.ExecuteNonQueryParamAsync(sqlQuery, value);

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
            param.Add("ip_client", HttpContext.Connection.RemoteIpAddress?.ToString() ?? "");
            param.Add("web_session", HttpContext.Session.Id);
            string sqlStr = $@"SELECT id, code, try_count, email FROM email_tasks WHERE 
                                      web_session=@web_session AND 
                                      ip_client=@ip_client AND 
                                      try_count<3 ORDER BY id DESC LIMIT 1;";
            var values = await Db.GetDb.GetRawQueryResultAsync(sqlStr, param);
            if (values.Count==0) return BadRequest(new
            {
                error = "Error confirmation",
                details = "В системе нет данных, что вы запрашивали подтверждение для какой-либо почты"
            });


            long try_count = (long)values[0]["try_count"];
            try_count++;

            param = new Dictionary<string, object>();
            param.Add("try_count", try_count);
            param.Add("id", values[0]["id"]);

            sqlStr = $@" UPDATE email_tasks  SET try_count=@try_count WHERE id=@id;";
            await Db.GetDb.ExecuteNonQueryParamAsync(sqlStr, param);

            if (dto.Code != values[0]["code"].ToString()) return BadRequest(new
            {
                error = "Spam detected",
                details = "С хоста запрещено делать более 5 записей в минуту, более 10 записей в 10 минут 20 записей в течение 4 часов"
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

            return Ok(new { status = "code confirm" });
        }






    }



}
