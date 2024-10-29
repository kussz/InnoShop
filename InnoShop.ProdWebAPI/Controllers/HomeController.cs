using InnoShop.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace InnoShop.ProdWebAPI.Controllers
{
    public class HomeController(DBInitializer initializer) : Controller
    {
        DBInitializer _initializer = initializer;
        [HttpGet]
        public IActionResult Index()
        {
            var ip = HttpContext.Request.Host.ToString();
            return View();
        }
        [HttpGet]
        public IActionResult FillDb()
        {
            try
            {
                string s = _initializer.DeleteFromTable("Products");
                s += _initializer.DeleteFromTable("ProdTypes");
                s += _initializer.DeleteFromTable("Users");
                s += _initializer.DeleteFromTable("UserTypes");
                s += _initializer.DeleteFromTable("Localities");
                s += _initializer.FillUserTypes();
                s += _initializer.FillLocalities(500);
                s += _initializer.FillUsers(1920);
                s += _initializer.FillProductTypes();
                s += _initializer.FillProducts(1000, 1920, 21);
                ViewData["log"] = s;
                return View();
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_Error.cshtml", ex);
            }
        }
    }
}
