using Azure;
using InnoShop.Domain.Models;
using InnoShop.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace InnoShop.Frontend.Controllers
{
    public class AccountController(HttpClient httpClient) : Controller
    {
        private readonly HttpClient _httpClient = httpClient;
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        // GET: Account/Detail/5
        public ActionResult Detail(int id)
        {
            var response = _httpClient.GetAsync($"http://localhost:5069/User/GetUser/{id}");
            response.Result.EnsureSuccessStatusCode();
            var user = response.Result.Content.ReadFromJsonAsync<User>().Result;
            return View(user);
        }
        [HttpPost]
        public ActionResult Login(UserLoginDTO userLogin)
        {
            var userString = JsonSerializer.Serialize(userLogin);
            var content = new StringContent(userString, Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync($"http://localhost:5069/User/Login", content).Result;
            response.EnsureSuccessStatusCode();
            //string resultString = response.Content.ReadAsStringAsync().Result;
            //Response.Cookies.Append("AspNetCore.Identity.Application", resultString);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Login()
        {
            return View();
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            var response = _httpClient.GetAsync($"http://localhost:5036/Locality/ForSelect");
            List<SelectListItem> localities = response.Result.Content.ReadFromJsonAsync<List<SelectListItem>>().Result;
            response = _httpClient.GetAsync($"http://localhost:5036/UserType/ForSelect");
            List<SelectListItem> userTypes = response.Result.Content.ReadFromJsonAsync<List<SelectListItem>>().Result;
            ViewBag.Localities = localities;
            ViewBag.Types= userTypes;
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegisterDTO user)
        {
            var userString = JsonSerializer.Serialize(user);
            var content = new StringContent(userString, Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync($"http://localhost:5069/User/Register", content);
            User createdUser = response.Result.Content.ReadFromJsonAsync<User>().Result;
            if (response.Result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Product"); // Перенаправление после успешного редактирования
            }
            else
            {
                response = _httpClient.GetAsync($"http://localhost:5036/Locality/ForSelect");
                SelectList localities = response.Result.Content.ReadFromJsonAsync<SelectList>().Result;
                response = _httpClient.GetAsync($"http://localhost:5036/UserType/ForSelect");
                SelectList userTypes = response.Result.Content.ReadFromJsonAsync<SelectList>().Result;
                ViewBag.Localities = localities;
                ViewBag.Types = userTypes;
                return View();
            }
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: Account/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Account/Delete/5
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
        public IActionResult ProtectedAction()
        {
            var userName = User.Identity.Name; // Это имя авторизованного пользователя
            return View(userName);
        }
    }
}
