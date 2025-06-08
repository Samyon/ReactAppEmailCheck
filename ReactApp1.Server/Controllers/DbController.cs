using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReactApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbController : ControllerBase
    {
        [HttpGet(Name = "GetDb")]
        public async Task<string> GetDb()
        {
            var gstr = new GetStr();
            return await gstr.GetStr1("");
        }



    }
}
