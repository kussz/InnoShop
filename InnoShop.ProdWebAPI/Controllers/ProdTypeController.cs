using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using InnoShop.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace InnoShop.ProdWebAPI.Controllers
{
    public class ProdTypeController(IServiceManager service) : Controller
    {
        IServiceManager _service = service;
        [HttpGet]
        [Authorize(Roles = "Admin")]
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
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(ms => ms.Value.Errors.Count > 0)
    .SelectMany(ms => ms.Value.Errors.Select(e => e.ErrorMessage))
    .ToList();
                return BadRequest(new { Errors = errors });
            }
            var locality = new ProdType() { Id = localityDTO.Id, Name = localityDTO.Name };
            try
            {
                _service.ProdTypeService.Edit(locality);

            }
            catch (DbUpdateException dbEx)
            {
                var sqlException = dbEx.InnerException as SqlException;
                if (sqlException != null && sqlException.Number == 2601) // Код ошибки для уникального ключа
                {
                    return BadRequest(new { Errors = new[] { $"Ошибка: категория с именем '{localityDTO.Name}' уже существует. Пожалуйста, выберите другое имя." } });
                }
                return BadRequest(new { Errors = new[] { $"Ошибка при сохранении данных: {dbEx.Message}" } });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Errors = new[] { $"Произошла ошибка: {ex.Message}" } });
            }
            return Ok(locality);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] LocalityEditDTO localityDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(ms => ms.Value.Errors.Count > 0)
    .SelectMany(ms => ms.Value.Errors.Select(e => e.ErrorMessage))
    .ToList();
                return BadRequest(new { Errors = errors });
            }
            var locality = new ProdType() { Id = localityDTO.Id, Name = localityDTO.Name };
            try
            {

                _service.ProdTypeService.Add(locality);
            }
            catch (DbUpdateException dbEx)
            {
                var sqlException = dbEx.InnerException as SqlException;
                if (sqlException != null && sqlException.Number == 2601) // Код ошибки для уникального ключа
                {
                    return BadRequest(new { Errors = new[] { $"Ошибка: категория с именем '{localityDTO.Name}' уже существует. Пожалуйста, выберите другое имя." } });
                }
                return BadRequest(new { Errors = new[] { $"Ошибка при сохранении данных: {dbEx.Message}" } });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Errors = new[] { $"Произошла ошибка: {ex.Message}" } });
            }
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
