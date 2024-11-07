using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InnoShop.ProdWebAPI.Controllers
{
    public class LocalityController(HttpClient client) : Controller
    {
        HttpClient _httpClient = client;
        [HttpGet]
        public IActionResult Index()
        {
            var response = _httpClient.GetAsync("http://localhost:5036/Locality" + Request.Query["page"]);
            response.Result.EnsureSuccessStatusCode();
            var localities = response.Result.Content.ReadFromJsonAsync<List<Locality>>().Result;
            return View(localities);
        }
    }
}
