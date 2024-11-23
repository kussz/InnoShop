using InnoShop.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using InnoShop.DTO;
using System.Net.Http;
using System.Text.Json;
using InnoShop.DTO.Models;
using System.Text;

namespace InnoShop.Frontend.Controllers
{
    public class ProductController(HttpClient httpClient) : Controller
    {
        private readonly HttpClient _httpClient = httpClient;
        // GET: ProductController
        public ActionResult Index()
        {
            var response = _httpClient.GetAsync($"http://localhost:5036/ProdType/");
            response.Result.EnsureSuccessStatusCode();
            var product = response.Result.Content.ReadFromJsonAsync<IEnumerable<ProdType>>().Result;
            ProductFilterDTO filterDTO = new()
            {
                Categories = product,
                MinPrice = null,
                MaxPrice = null,
            };


            return View(filterDTO);
        }

        // GET: ProductController/Details/5
        public ActionResult Detail(int id)
        {
            var response = _httpClient.GetAsync($"http://localhost:5036/Product/Details/{id}");
            response.Result.EnsureSuccessStatusCode();
            var product = response.Result.Content.ReadFromJsonAsync<Product>().Result;
            return View(product);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            var response = _httpClient.GetAsync($"http://localhost:5036/Product/Create/");
            response.Result.EnsureSuccessStatusCode();
            var data = response.Result.Content.ReadFromJsonAsync<ProductEditData>().Result;
            ViewBag.Categories = data.Categories;
            ViewBag.Users = data.Users;
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            var productString = JsonSerializer.Serialize(product);
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync($"http://localhost:5036/Product/Create", content);
            if (response.Result.IsSuccessStatusCode)
            {
                Product createdProduct = response.Result.Content.ReadFromJsonAsync<Product>().Result;
                return RedirectToAction("Detail", new { id = createdProduct.Id }); // Перенаправление после успешного редактирования
            }
            else
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            var response = _httpClient.GetAsync($"http://localhost:5036/Product/Edit/{id}");
            response.Result.EnsureSuccessStatusCode();
            var data = response.Result.Content.ReadFromJsonAsync<ProductEditData>().Result;
            var product = data.Product;
            ViewBag.Categories = data.Categories;
            ViewBag.Users = data.Users;
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            var productString = JsonSerializer.Serialize(product);
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync($"http://localhost:5036/Product/Edit", content);
            if (response.Result.IsSuccessStatusCode)
            {
                return RedirectToAction("Detail",new { id = product.Id }); // Перенаправление после успешного редактирования
            }
            else
            {
                return StatusCode((int) response.Result.StatusCode, "Ошибка при редактировании продукта");
            }
}

        // GET: ProductController/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(id);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
