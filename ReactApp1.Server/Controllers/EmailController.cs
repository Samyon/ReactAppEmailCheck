using Microsoft.AspNetCore.Mvc;
using System;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {

        [HttpPost("api/email")]
        public IActionResult ReceiveEmail([FromBody] EmailDto dto)
        {
            Console.WriteLine("Received email: " + dto.Email);
            return Ok(new { status = "received" });
        }

        public class EmailDto
        {
            public string Email { get; set; }
        }


    }
}
