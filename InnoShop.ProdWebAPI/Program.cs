using InnoShop.Application;
using InnoShop.Contracts.Repository;
using InnoShop.Contracts.Service;
using InnoShop.Domain.Data;
using InnoShop.Domain.Models;
using InnoShop.DTO.Models;
using InnoShop.Infrastructure;
using InnoShop.Infrastructure.Initialize;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
                options.AddPolicy("AllowLocalhost5030", builde =>
                {
                    builde
                        .WithOrigins(builder.Configuration.GetSection("HostSettings")["Frontend"])  // Укажите источник фронтенда
                        .AllowCredentials()                    // Разрешить передачу учетных данных
                        .AllowAnyHeader()                      // Разрешить любые заголовки
                        .AllowAnyMethod();                     // Разрешить любые методы (GET, POST, PUT, и т.д.)
                });
            });
            builder.Services.AddIdentity<User, IdentityRole<int>>()
            .AddEntityFrameworkStores<InnoShopContext>()
            .AddDefaultTokenProviders();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        var JWTSettings = builder.Configuration.GetSection("JWTSettings");
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            // укзывает, будет ли валидироваться издатель при валидации токена
                            ValidateIssuer = true,
                            // строка, представляющая издателя
                            ValidIssuer = JWTSettings["Issuer"],

                            // будет ли валидироваться потребитель токена
                            ValidateAudience = true,
                            // установка потребителя токена
                            ValidAudience = JWTSettings["Audience"],
                            // будет ли валидироваться время существования
                            ValidateLifetime = true,

                            // установка ключа безопасности
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWTSettings["SecretKey"])),
                            // валидация ключа безопасности
                            ValidateIssuerSigningKey = true,
                        };
                    });


            builder.Services.AddAuthorization();
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.Configure<HostSettings>(builder.Configuration.GetSection("HostSettings"));
            builder.Services.AddScoped<DBInitializer>();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddMemoryCache();
            builder.WebHost.UseStaticWebAssets();

            var app = builder.Build();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowLocalhost5030");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}
