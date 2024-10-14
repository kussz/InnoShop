using Microsoft.Extensions.Primitives;
using Microsoft.EntityFrameworkCore;
using InnoShop.Domain.Data;
using InnoShop.Domain.Models;
using InnoShop.Application;
using InnoShop.Contracts.Service;
using InnoShop.Contracts.Repository;
using InnoShop.Infrastructure.Repositories;
using InnoShop.Infrastructure;

namespace InnoShop.ProdWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DBConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<InnoShopContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddMemoryCache();
            var app = builder.Build();

            app.MapGet("/user/{id}", async (int id, IServiceManager service) =>
            {
                User? user = service.UserService.GetUser(id);
                if (user != null) return $"User {user.Login}  Id={user.Id}  Mail={user.Email}";
                return "User not found";
            });
            app.Run();
        }

        private static void HandleId(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                StringValues a;
                context.Request.Query.TryGetValue("id", out a);
                await context.Response.WriteAsync(a);
            });


        }
        private static void About(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("About");
            });

        }
        private static void Index(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Index");
            });
        }
        private static void Home(IApplicationBuilder app)
        {
            app.Run(async context => {
                await context.Response.WriteAsync("It's home");
            }
            );
        }

    }
}
