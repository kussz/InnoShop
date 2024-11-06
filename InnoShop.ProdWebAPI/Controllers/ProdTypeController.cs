using InnoShop.Contracts.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult ForSelect()
        {
            return Ok(_service.ProdTypeService.GetAllProdTypes().Select(pt => new SelectListItem { Value = pt.Id.ToString(), Text = pt.Name }));
        }
    }
}
