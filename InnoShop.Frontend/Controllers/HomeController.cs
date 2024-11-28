using InnoShop.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;

namespace InnoShop.Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        public IActionResult NavPartial()
        {
            return PartialView("~/Views/Shared/_NavPartial.cshtml");
        }
        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            // Получаем сообщение об ошибке из TempData
            var errorMessage = TempData["Exception"] as string;

            // Если сообщение отсутствует, можно вернуть общее сообщение об ошибке
            if (string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = "Произошла неизвестная ошибка.";
            }

            return View("Error", errorMessage); // Передаем сообщение в представление
        }
    }
}
