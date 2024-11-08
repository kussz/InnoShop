
using Microsoft.AspNetCore.Identity;
using InnoShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using InnoShop.Domain.Data;
using InnoShop.Application;
using InnoShop.Contracts.Repository;
using InnoShop.Contracts.Service;
using InnoShop.Infrastructure.Initialize;
using InnoShop.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace InnoShop.UserWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("newDBConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<InnoShopContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost5030", builder =>
                {
                    builder
                        .WithOrigins("http://localhost:5030")  // Укажите источник фронтенда
                        .AllowCredentials()                    // Разрешить передачу учетных данных
                        .AllowAnyHeader()                      // Разрешить любые заголовки
                        .AllowAnyMethod();                     // Разрешить любые методы (GET, POST, PUT, и т.д.)
                });
            });
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "AspNetCore.Identity.Application";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None; // Для кросс-доменного использования
                options.Cookie.SecurePolicy = CookieSecurePolicy.None; // Для localhost можно использовать HTTP
                options.LoginPath = "/User/Login";
            });


            builder.Services.AddAuthorization();
            builder.Services.AddIdentity<User, IdentityRole<int>>()
            .AddEntityFrameworkStores<InnoShopContext>()
            .AddDefaultTokenProviders();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });
            
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddScoped<DBInitializer>();
            builder.WebHost.UseStaticWebAssets();
            var app = builder.Build();

            
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowLocalhost5030");
            app.UseAuthentication();
            app.UseAuthorization();
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
