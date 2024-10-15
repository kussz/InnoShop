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
using System.Text;
using Microsoft.AspNetCore.Http;

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
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddMemoryCache();
            var app = builder.Build();
            app.UseSession();
            app.Map("/", async context =>
            {
                await context.Response.WriteAsync("Nothing to show");
            });

            app.Map("/info", async (context) =>
            {
                string info = $"Host: {context.Request.Host}\nPath: {context.Request.Path}\nProtocol: {context.Request.Protocol}";
                await context.Response.WriteAsync(info);
            });
            app.MapGet("/products", async (context) =>
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
            app.Map("/searchform1",async( context) =>
            {
                    string? locality = "";
                    if (context.Request.Cookies.ContainsKey("locality"))
                    {
                        locality = context.Request.Cookies["locality"];
                    }
                    int? prodTypeId = 1;
                    if (context.Request.Cookies.ContainsKey("prodTypeId"))
                    {
                        prodTypeId = Int32.Parse(context.Request.Cookies["prodTypeId"]);
                    }
                    context.Items["locality"] = locality;
                    context.Items["prodTypeId"] = prodTypeId;
                    await FormHandler(context);
            });
            app.Map("/searchform2", async( context )=>
            {
                    string? locality = "";
                    if (context.Session.Keys.Contains("locality"))
                    {
                        locality = context.Session.GetString("locality");
                    }
                    int? prodTypeId = 1;
                    if (context.Session.Keys.Contains("prodTypeId"))
                    {
                        prodTypeId = context.Session.GetInt32("prodTypeId");
                    }
                    context.Items["locality"] = locality;
                    context.Items["prodTypeId"] = prodTypeId;
                    await FormHandler(context);

            });
            app.Map("/products/{id}", (int id, IServiceManager service) =>
            {
                var product = service.ProductService.GetProduct(id);
                return $"Id: {product.Id}\nName: {product.Name}\nUser: {product.User.Login}\nCost: {product.Cost}";
            });
            app.MapGet("/localities", async (context) =>
            {
                string locality = "";
                var service = context.RequestServices.GetService<IServiceManager>();
                locality = context.Request.Query["locality"];
                int prodTypeId = Int32.Parse( context.Request.Query["ProdType"]);
                context.Response.Cookies.Append("locality", locality);
                context.Response.Cookies.Append("prodTypeId", prodTypeId.ToString());
                context.Session.SetString("locality", locality);
                context.Session.SetInt32("prodTypeId", prodTypeId);
                string response = "<HTML><HEAD>" +
                "<meta charset=\"utf-8\" />" +
                "<TITLE>Продукты</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
                $"<BODY><H1>Список продуктов в городах с \"{locality}\"</H1>" +
                "<table border=1><tr><th>Название</th><th>Id</th><th>Цена</th><th>Населенный пункт</th><th>Пользователь</th><th>Категория</th></tr>";
                List<Product>? products = service.ProductService.GetProductsByCondition(p =>p.ProdTypeId == prodTypeId&& p.User.Locality.Name.ToUpper().Contains(locality.ToUpper()));
                if (products != null)
                    foreach (var product in products)
                        response += $"<tr><td>{product.Name}</td><td>{product.Id}</td><td>{product.Cost}</td><td>{product.User.Locality.Name}</td><td>{product.User.Login}</td><td>{product.ProdType.Name}</td></tr>";
                await context.Response.WriteAsync(response + "</table></body></html>");
            });
            app.Run();
        }
        private async static Task FormHandler(HttpContext context)
        {
            string locality = context.Items["locality"].ToString();
            int? prodTypeId = Int32.Parse(context.Items["prodTypeId"].ToString());
            context.Items.Remove("locality");
            context.Items.Remove("prodTypeId");
            var service = context.RequestServices.GetRequiredService<IServiceManager>();
            string response = "<html><meta charset=\"utf-8\" /><body><form action = 'http://localhost:5036/localities'/ >" +
            "Locality: <input type = 'text' name = 'locality' value = " + locality + ">" +
            $"<br>Product type: <select name='prodType'";
            foreach (var type in service.ProdTypeService.GetAllProdTypes())
                response += $"<option {(type.Id==prodTypeId?"selected":"")} value='{type.Id}'>{type.Name}</option>";
            response+="</select><br><br><input type = 'submit' value = 'Сохранить +куки' ></form></body></html>";

            await context.Response.WriteAsync(response);
        }

    }
}
