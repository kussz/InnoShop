using Microsoft.AspNetCore.Mvc;

namespace InnoShop.ProdWebAPI.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var ip = HttpContext.Request.Host.ToString();
            return View();
        }
        [HttpGet]
        public IActionResult Index1()
        {
            var ip = HttpContext.Request.Host.ToString();
            return View();
        }
    }
}
