using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using InnoShop.DTO.Models;
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
        public IActionResult Details(int id)
        {
            var prodType = _service.ProdTypeService.GetProdType(id);
            if (prodType != null)
                return Ok(prodType);
            else
                return NotFound();
        }
        [HttpPost]
        public IActionResult Details([FromBody] LocalityEditDTO localityDTO)
        {
            var locality = new ProdType() { Id = localityDTO.Id, Name = localityDTO.Name };
            _service.ProdTypeService.Edit(locality);
            return Ok(locality);
        }
        public IActionResult ForSelect()
        {
            return Ok(new SelectList(_service.ProdTypeService.GetAllProdTypes(), "Id", "Name"));
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var prodType = _service.ProdTypeService.GetProdType(id);
            _service.ProdTypeService.Remove(prodType);
            return NoContent();
        }
    }
}
