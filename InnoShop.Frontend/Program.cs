using InnoShop.Contracts.Service;
using InnoShop.Domain.Data;
using InnoShop.Domain.Models;
using InnoShop.Frontend.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace InnoShop.Frontend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "AspNetCore.Identity.Application";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.Cookie.SameSite = SameSiteMode.None;  // или SameSiteMode.Strict
            });
            builder.Services.AddAuthorization();

            
            builder.Services.AddHttpClient("WithCookies", client =>
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.WebHost.UseStaticWebAssets();
            
            var app = builder.Build();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
