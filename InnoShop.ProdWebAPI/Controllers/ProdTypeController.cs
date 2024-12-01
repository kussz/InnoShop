using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using InnoShop.DTO.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        public IActionResult Details(int id)
        {
            var prodType = _service.ProdTypeService.GetProdType(id);
            if (prodType != null)
                return Ok(prodType);
            else
                return NotFound();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit([FromBody] LocalityEditDTO localityDTO)
        {
            var locality = new ProdType() { Id = localityDTO.Id, Name = localityDTO.Name };
            _service.ProdTypeService.Edit(locality);
            return Ok(locality);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] LocalityEditDTO localityDTO)
        {
            var locality = new ProdType() { Id = localityDTO.Id, Name = localityDTO.Name };
            _service.ProdTypeService.Add(locality);
            return Ok(locality);
        }
        public IActionResult ForSelect()
        {
            return Ok(new SelectList(_service.ProdTypeService.GetAllProdTypes(), "Id", "Name"));
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var prodType = _service.ProdTypeService.GetProdType(id);
            _service.ProdTypeService.Remove(prodType);
            return NoContent();
        }
    }
}
