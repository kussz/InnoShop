using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using InnoShop.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace InnoShop.ProdWebAPI.Controllers
{
    public class LocalityController(IServiceManager service) : Controller
    {
        IServiceManager _service = service;
        [HttpGet]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public IActionResult Details(int id)
        {
            var locality = _service.LocalityService.GetLocality(id);
            if (locality != null)
                return Ok(locality);
            else
                return NotFound();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var locality = _service.LocalityService.GetLocality(id);
            _service.LocalityService.Remove(locality);
            return NoContent();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit([FromBody]LocalityEditDTO localityDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(ms => ms.Value.Errors.Count > 0)
    .SelectMany(ms => ms.Value.Errors.Select(e => e.ErrorMessage))
    .ToList();
                return BadRequest(new { Errors = errors });
            }
            try
            {
                var locality = new Locality() { Id = localityDTO.Id, Name = localityDTO.Name };
                _service.LocalityService.Edit(locality);
                return Ok(locality);
            }
            catch (DbUpdateException dbEx)
            {
                var sqlException = dbEx.InnerException as SqlException;
                if (sqlException != null && sqlException.Number == 2601) // Код ошибки для уникального ключа
                {
                    return BadRequest(new { Errors = new[] { $"Ошибка: населённый пункт с именем '{localityDTO.Name}' уже существует. Пожалуйста, выберите другое имя." } });
                }
                return BadRequest(new { Errors = new[] { $"Ошибка при сохранении данных: {dbEx.Message}" } });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Errors = new[] { $"Произошла ошибка: {ex.Message}" } });
            }
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
            try
            {
                var locality = new Locality() { Id = localityDTO.Id, Name = localityDTO.Name };
                _service.LocalityService.Add(locality);
                return Ok(locality);
            }
            catch (DbUpdateException dbEx)
            {
                var sqlException = dbEx.InnerException as SqlException;
                if (sqlException != null && sqlException.Number == 2601) // Код ошибки для уникального ключа
                {
                    return BadRequest(new { Errors = new[] { $"Ошибка: населённый пункт с именем '{localityDTO.Name}' уже существует. Пожалуйста, выберите другое имя." } });
                }
                return BadRequest(new { Errors = new[] { $"Ошибка при сохранении данных: {dbEx.Message}" } });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Errors = new[] { $"Произошла ошибка: {ex.Message}" } });
            }
        }
        public IActionResult ForSelect()
        {
            return Ok(new SelectList(_service.LocalityService.GetAllLocalities(), "Id", "Name"));
        }
    }
}
