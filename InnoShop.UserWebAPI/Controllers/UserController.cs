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
using Microsoft.AspNetCore.Authorization;

namespace InnoShop.UserWebAPI.Controllers
{
    public class UserController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, SignInManager<User> signInManager, IServiceManager service,IOptions<JwtSettings> jwtSettings, IConfiguration configuration) : Controller
    {
        UserManager<User> _userManager = userManager;
        SignInManager<User> _signInManager = signInManager;
        RoleManager<IdentityRole<int>> _roleManager = roleManager;
        IServiceManager _service = service;
        JwtSettings _jwtSettings = jwtSettings.Value;

        // GET: UserController

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
                new Claim("Id", user.Id.ToString()),
                new Claim("role", _userManager.GetRolesAsync(user).Result[0])
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );
            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody]string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if(!roleExists)
                await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
            return Ok(!roleExists);
        }
        [HttpPost]
        public async Task<IActionResult> SetRole([FromBody] UserAddRoleDTO userAddRole)
        {
            var roleExists = await _roleManager.RoleExistsAsync(userAddRole.RoleName);
            var user = _service.UserService.GetUser(userAddRole.Id);
            if (roleExists)
                await _userManager.AddToRoleAsync(user, userAddRole.RoleName);
            return Ok(user);
        }
        //[Authorize(Roles ="User, Admin")]
        public IActionResult GetProfile()
        {
            var user = _service.UserService.Authorize(Request.Headers["Authorization"]);
            // Получаем токен из заголовка Authorization
            if(user!=null)
            {
                return Ok(user);
            }
                return Unauthorized();
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
                    await SetRole(new UserAddRoleDTO() { Id = user.Id, RoleName = "User" });
                    var token = GenerateToken(user);
                    return Ok(token);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Unauthorized(new { Errors = result.Errors.Select(e => e.Description) });
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
