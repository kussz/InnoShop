using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using InnoShop.DTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InnoShop.ProdWebAPI.Controllers
{
    public class ProdAttribController(IServiceManager service) : Controller
    {
        IServiceManager _service = service;
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var attrib = _service.ProdAttribService.GetProdAttrib(id);
            _service.ProdAttribService.Remove(attrib);
            return NoContent();
        }
        [HttpPost]
        public IActionResult Create([FromBody] LocalityEditDTO localityDTO)
        {
            try
            {
                var attrib = new ProdAttrib() { Id = localityDTO.Id, Name = localityDTO.Name };
                _service.ProdAttribService.Add(attrib);
                return Ok(attrib);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
