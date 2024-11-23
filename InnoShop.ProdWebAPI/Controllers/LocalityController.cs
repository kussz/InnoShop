﻿using InnoShop.Contracts.Service;
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
        public IActionResult Delete(int id)
        {
            var locality = _service.LocalityService.GetLocality(id);
            _service.LocalityService.Remove(locality);
            return NoContent();
        }
        [HttpPost]
        public IActionResult Edit([FromBody]LocalityEditDTO localityDTO)
        {
            try
            {
                var locality = new Locality() { Id = localityDTO.Id, Name = localityDTO.Name };
                _service.LocalityService.Edit(locality);
                return Ok(locality);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        [HttpPost]
        public IActionResult Create([FromBody] LocalityEditDTO localityDTO)
        {
            try
            {
                var locality = new Locality() { Id = localityDTO.Id, Name = localityDTO.Name };
                _service.LocalityService.Add(locality);
                return Ok(locality);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        public IActionResult ForSelect()
        {
            return Ok(new SelectList(_service.LocalityService.GetAllLocalities(), "Id", "Name"));
        }
    }
}
