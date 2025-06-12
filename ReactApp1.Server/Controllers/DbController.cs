using Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReactApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbController : ControllerBase
    {
        [HttpGet(Name = "GetDb")]
        public async Task<string> GetDb1()
        {
            HttpContext.Session.SetString("MyKey", "Some value");
            //return Ok("Session value set.");


            var gstr = GetDb.GetPath();
            return gstr;
        }



    }
}
