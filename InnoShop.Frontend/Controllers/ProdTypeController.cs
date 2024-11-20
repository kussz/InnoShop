using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;

namespace InnoShop.ProdWebAPI.Controllers
{
    public class ProdTypeController(HttpClient client) : Controller
    {
        HttpClient _httpClient = client;
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            return View(id);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(id);
        }
    }
}
