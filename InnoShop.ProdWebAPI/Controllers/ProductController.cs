using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using InnoShop.DTO.Models;
using System.IdentityModel.Tokens.Jwt;

namespace InnoShop.ProdWebAPI.Controllers
{
    public class ProductController(IServiceManager service) : Controller
    {
        readonly IServiceManager _service = service;
        [HttpGet]
        public IActionResult Index(int page = 1)
        {
            try
            {
                var products = _service.ProductService.GetPage(30, page);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult ForUser()
        {
            var user = _service.UserService.Authorize(Request.Headers.Authorization);
            try
            {
                var products = _service.ProductService.GetProductsByCondition(p => p.UserId == user.Id).OrderByDescending(p=>p.CreationDate).ToList();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            ProductEditData data = new()
            {
                Categories = _service.ProdTypeService.GetAllProdTypes().Select(p => new SelectListItem(p.Name, p.Id.ToString())),
                Users = _service.UserService.GetAllUsers().Select(p => new SelectListItem(p.UserName, p.Id.ToString()))
            };
            return Ok(data);
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            var user = _service.UserService.Authorize(Request.Headers.Authorization);
            try
            {
                if (product.UserId == user.Id)
                {
                    _service.ProductService.Add(product);
                    return Ok(product);
                }
                else
                    return BadRequest();
            }
            catch(Exception ex) { return BadRequest(ex.Message); }
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            Product product = _service.ProductService.GetProduct(id);
            return Ok(product);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ProductEditData data = new()
            {
                Product = _service.ProductService.GetProduct(id),
                Categories = new SelectList(_service.ProdTypeService.GetAllProdTypes(),"Id","Name"),
                Users = new SelectList(_service.UserService.GetAllUsers(),"Id","UserName")
            };
            return Ok(data);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
                var role = _service.UserService.GetRole(Request.Headers.Authorization);
                var user = _service.UserService.Authorize(Request.Headers.Authorization);
                if (user.Id == product.UserId || role == "Admin")
                {
                    _service.ProductService.Edit(product);
                    return Ok(product);
                }
                else
                    return BadRequest();
            //try
            //{
            //}
            //catch (Exception ex) { return BadRequest(ex.Message); }

        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = _service.UserService.Authorize(Request.Headers.Authorization);
            var role = _service.UserService.GetRole(Request.Headers.Authorization);
            var product = _service.ProductService.GetProduct(id);
            try
            {
                if (user.Id == product.UserId || role == "Admin")
                {
                    _service.ProductService.Remove(product);
                    return NoContent();
                }
                else
                    return BadRequest();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
