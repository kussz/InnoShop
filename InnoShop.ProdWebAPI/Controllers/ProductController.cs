using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

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
                return View(_service.ProductService.GetPage(15,page));
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_Error.cshtml", ex);
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _service.ProdTypeService.GetAllProdTypes().Select(pt=>new SelectListItem { Value = pt.Id.ToString(), Text = pt.Name });
            ViewBag.Users = _service.UserService.GetAllUsers().Select(pt => new SelectListItem { Value = pt.Id.ToString(), Text = pt.Login });
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            _service.ProductService.Add(product);
            return RedirectToAction("Detail",new { id = product.Id });
        }
        [HttpGet]
        public IActionResult Detail(int id)
        {
            Product product = _service.ProductService.GetProduct(id);
            return View(product);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Product product = _service.ProductService.GetProduct(id);
            ViewBag.Categories = _service.ProdTypeService.GetAllProdTypes().Select(pt => new SelectListItem { Value = pt.Id.ToString(), Text = pt.Name });
            ViewBag.Users = _service.UserService.GetAllUsers().Select(pt => new SelectListItem { Value = pt.Id.ToString(), Text = pt.Login });
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            _service.ProductService.Edit(product);
            return RedirectToAction("Detail", new { id = product.Id });
        }
    }
}
