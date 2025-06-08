using Microsoft.AspNetCore.Mvc;
using System;

namespace ReactApp1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {

        //[HttpPost]
        //public async Task<IActionResult> PostEmail([FromBody] EmailEntry entry)
        //{
        //    if (!ModelState.IsValid) return BadRequest();

        //    _db.Emails.Add(entry);
        //    await _db.SaveChangesAsync();
        //    return Ok(new { success = true });
        //}
    }
}
