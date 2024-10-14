using Microsoft.Extensions.Primitives;
using Microsoft.EntityFrameworkCore;
using InnoShop.Domain.Data;
using InnoShop.Domain.Models;
using InnoShop.Application;
using InnoShop.Contracts.Service;
using InnoShop.Contracts.Repository;
using InnoShop.Infrastructure.Repositories;
using InnoShop.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;

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
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddMemoryCache();
            var app = builder.Build();



            app.Map("/info", async (context) =>
            {
                await context.Response.WriteAsync( "IP: "+context.Connection.RemoteIpAddress.ToString()+"\nUser:"+context.User.ToString());
            });
            app.MapGet("/table", async (context) =>
            {
                    var mng = context.RequestServices.GetService<IServiceManager>();
                    var prods = mng.ProductService.GetCachedProducts();
                    string response = "<HTML><HEAD>" +
                "<TITLE>Продукты</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
                "<BODY><H1>Список кэшированных записей</H1>"+
                "<table border=1><tr><th>Id</th><th>Название</th><th>Описание</th></tr>";
                    foreach (var prod in prods)
                        response += "<tr><td>" + prod.Id +"</td><td>"+prod.Name + "</td><td>" +prod.Description + "</td></tr>";
                    await context.Response.WriteAsync(response + "</table></body></html>");
            });
            app.Map("/searchform1", FormHandler);
            app.Map("/searchform2", FormHandler);
            app.MapGet("/localities", async (context) =>
            {
                string locality = "";
                var service = context.RequestServices.GetService<IServiceManager>();
                locality = context.Request.Query["locality"];
                string response = "<HTML><HEAD>" +
                "<meta charset=\"utf-8\" />" +
                "<TITLE>Продукты</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
                $"<BODY><H1>Список продуктов в городах с \"{locality}\"</H1>" +
                "<table border=1><tr><th>Название</th><th>Id</th><th>Цена</th><th>Населенный пункт</th><th>Пользователь</th></tr>";
                List<Product>? products = service.ProductService.GetProductsByCondition(p => p.User.Locality.Name.ToUpper().Contains(locality.ToUpper()));
                if (products != null)
                    foreach (var product in products)
                        response += $"<tr><td>{product.Name}</td><td>{product.Id}</td><td>{product.Cost}</td><td>{product.User.Locality.Name}</td><td>{product.User.Login}</td></tr>";
                await context.Response.WriteAsync(response + "</table></body></html>");
            });
            app.Run();
        }
        private static void FormHandler(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                string locality = "";
                locality = context.Request.Query["locality"];

                var service = context.RequestServices.GetRequiredService<IServiceManager>();
                string response = "<html><meta charset=\"utf-8\" /><body><form action = 'http://localhost:5036/localities'/ >" +
                "Locality: <input type = 'text' name = 'locality' value = " + locality + ">" +
                "<br>Product type: <select name='prodType'";
                foreach (var type in service.ProdTypeService.GetAllProdTypes())
                    response += $"<option value='{type.Id}'>{type.Name}</option>";
                response+="</select><br><br><input type = 'submit' value = 'Submit ' ></form></body></html>";

                await context.Response.WriteAsync(response);
            });
        }

    }
}
