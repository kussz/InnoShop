using Microsoft.AspNetCore.Mvc;

namespace InnoShop.ProdWebAPI.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            return Ok(ip);
        }
    }
}
