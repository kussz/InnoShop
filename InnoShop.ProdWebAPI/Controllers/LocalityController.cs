using InnoShop.Contracts.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InnoShop.ProdWebAPI.Controllers
{
    public class LocalityController(IServiceManager service) : Controller
    {
        IServiceManager _service = service;
        [HttpGet]
        public IActionResult Index()
        {
            try
            {

            return Ok(_service.LocalityService.GetAllLocalities());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        public IActionResult ForSelect()
        {
            return Ok(new SelectList(_service.LocalityService.GetAllLocalities(), "Id", "Name"));
        }
    }
}
