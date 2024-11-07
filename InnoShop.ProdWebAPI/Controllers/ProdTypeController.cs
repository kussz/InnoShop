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

            return Ok(_service.ProdTypeService.GetAllProdTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        public IActionResult ForSelect()
        {
            return Ok(new SelectList(_service.ProdTypeService.GetAllProdTypes(), "Id", "Name"));
        }
    }
}
