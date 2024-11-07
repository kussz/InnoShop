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
            var response = _httpClient.GetAsync("http://localhost:5036/ProdType" + Request.Query["page"]);
            response.Result.EnsureSuccessStatusCode();
            var prodtypes = response.Result.Content.ReadFromJsonAsync<List<ProdType>>().Result;
            return View(prodtypes);
        }
    }
}
