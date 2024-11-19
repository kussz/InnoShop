using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using InnoShop.DTO.Models;
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
        public IActionResult Details(int id)
        {
            var locality = _service.LocalityService.GetLocality(id);
            if (locality != null)
                return Ok(locality);
            else
                return NotFound();
        }
        [HttpPost]
        public IActionResult Details([FromBody]LocalityEditDTO localityDTO)
        {
            var locality = new Locality() { Id = localityDTO.Id, Name = localityDTO.Name };
            _service.LocalityService.Edit(locality);
            return Ok(locality);
        }
        public IActionResult ForSelect()
        {
            return Ok(new SelectList(_service.LocalityService.GetAllLocalities(), "Id", "Name"));
        }
    }
}
