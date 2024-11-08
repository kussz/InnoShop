using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using InnoShop.DTO.Models;
using System.Text;
using System.Security.Cryptography;
using System.Net;

namespace InnoShop.UserWebAPI.Controllers
{
    public class UserController(UserManager<User> userManager, SignInManager<User> signInManager, IServiceManager service) : Controller
    {
        UserManager<User> _userManager = userManager;
        SignInManager<User> _signInManager = signInManager;
        IServiceManager _service = service;
        // GET: UserController
        public ActionResult GetCurrentUser()
        {
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(userId);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Login([FromBody]UserLoginDTO userLogin)
        {
            var result = await _signInManager.PasswordSignInAsync(userLogin.UserName, userLogin.Password,true,false);

            if (result.Succeeded)
            {
                return Ok();
                //return RedirectToAction(nameof(ReturnUser));
                
            }
            else
                return Unauthorized(userLogin);
        }
        [HttpGet]
        public ActionResult ReturnUser()
        {
            var cookieValue = HttpContext.Request.Cookies["AspNetCore.Identity.Application"];
            HttpContext.Response.Headers.Add("Set-Cookie", cookieValue);
            return Ok(cookieValue);
        }
        public async Task<IActionResult> Logout()
        {
            throw new NotImplementedException();
        }
        // POST: UserController/Register
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromBody]UserRegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.UserName, UserTypeId = model.UserTypeId, LocalityId = model.LocalityId, PasswordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(model.PasswordConfirm))), };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return Ok(user);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return Unauthorized(model);
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
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

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
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
        [HttpGet]
        public ActionResult GetUs()
        {
            return Ok(User.Identity.Name);
        }
        
    }
}
