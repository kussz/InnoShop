using InnoShop.Contracts.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace InnoShop.ProdWebAPI.Controllers
{
    public class ProductController(IServiceManager service) : Controller
    {
        IServiceManager _service = service;
        [HttpGet]
        public IActionResult Index(int page = 1)
        {
            try
            {
                ViewData["page"] = page;              
                return View(_service.ProductService.GetPage(15,page));
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_Error.cshtml", ex);
            }
        }
    }
}
