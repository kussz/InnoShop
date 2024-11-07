using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using InnoShop.DTO.Models;

namespace InnoShop.ProdWebAPI.Controllers
{
    public class ProductController(IServiceManager service) : Controller
    {
        IServiceManager _service = service;
        [HttpGet]
        public IActionResult Index(int page = 1)
        {
            try
            {
                var products = _service.ProductService.GetPage(15, page);
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
            ProductEditData data = new ProductEditData()
            {
                Categories = _service.ProdTypeService.GetAllProdTypes().Select(p => new SelectListItem(p.Name, p.Id.ToString())),
                Users = _service.UserService.GetAllUsers().Select(p => new SelectListItem(p.UserName, p.Id.ToString()))
            };
            return Ok(data);
        }
        [HttpPost]
        public IActionResult Create([FromBody]Product product)
        {
            _service.ProductService.Add(product);
            return Ok(product);
        }
        [HttpGet]
        public IActionResult GetProduct(int id)
        {
            Product product = _service.ProductService.GetProduct(id);
            return Ok(product);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ProductEditData data = new ProductEditData()
            {
                Product = _service.ProductService.GetProduct(id),
                Categories = new SelectList(_service.ProdTypeService.GetAllProdTypes(),"Id","Name"),
                Users = new SelectList(_service.UserService.GetAllUsers(),"Id","UserName")
            };
            return Ok(data);
        }
        [HttpPost]
        public IActionResult Edit([FromBody]Product product)
        {
            _service.ProductService.Edit(product);
            return Ok();
        }
    }
}
