using InnoShop.Contracts.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InnoShop.ProdWebAPI.Controllers
{
    public class ProdTypeController(IServiceManager service) : Controller
    {
        IServiceManager _service = service;
        [HttpGet]
        public IActionResult Index()
        {
            try
            {

            return View(_service.ProdTypeService.GetAllProdTypes());
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_Error.cshtml", ex);
            }
        }
    }
}
