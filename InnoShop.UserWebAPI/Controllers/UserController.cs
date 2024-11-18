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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Principal;
using System;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;

namespace InnoShop.UserWebAPI.Controllers
{
    public class UserController(UserManager<User> userManager, SignInManager<User> signInManager, IServiceManager service,IOptions<JwtSettings> jwtSettings, IConfiguration configuration) : Controller
    {
        UserManager<User> _userManager = userManager;
        SignInManager<User> _signInManager = signInManager;
        IServiceManager _service = service;
        JwtSettings _jwtSettings = jwtSettings.Value;
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
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = GenerateToken(user);

                return Ok(token);
            }

            return Unauthorized();
        }
        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpGet("Profile")]
        public IActionResult GetProfile()
        {
            // Получаем токен из заголовка Authorization
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Проверяем, что токен не пустой
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            // Декодируем токен
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            var id = int.Parse(jwtToken?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            return Ok(_service.UserService.GetUser(id));
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
