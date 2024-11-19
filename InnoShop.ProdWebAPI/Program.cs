using InnoShop.Application;
using InnoShop.Contracts.Repository;
using InnoShop.Contracts.Service;
using InnoShop.Domain.Data;
using InnoShop.Domain.Models;
using InnoShop.Infrastructure;
using InnoShop.Infrastructure.Initialize;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InnoShop.ProdWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("newDBConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<InnoShopContext>(options =>
                options.UseSqlServer(connectionString,b => b.MigrationsAssembly("InnoShop.Domain")));
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
            builder.Services.AddIdentity<User, IdentityRole<int>>()
            .AddEntityFrameworkStores<InnoShopContext>()
            .AddDefaultTokenProviders();
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddScoped<DBInitializer>();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddMemoryCache();
            builder.WebHost.UseStaticWebAssets();

            var app = builder.Build();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowLocalhost5030");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}
