﻿using UMS.Contracts;
using Microsoft.EntityFrameworkCore;
using Repository;
using PMS.Service;
using UMS.LoggerService;
using PMS.Service.Contracts;
using Microsoft.AspNetCore.Identity;
using UMS.Entities.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PMS.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace UMS.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("X-Pagination"));
        });
    public static void ConfigureIISIntegration(this IServiceCollection services) =>
        services.Configure<IISOptions>(options =>
        {
        });
    public static void ConfigureLoggerService(this IServiceCollection services) =>
 services.AddSingleton<ILoggerManager, LoggerManager>();
    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
     services.AddScoped<IRepositoryManager, RepositoryManager>();
    public static void ConfigureServiceManager(this IServiceCollection services) =>
services.AddScoped<IServiceManager, ServiceManager>();
    public static void ConfigureSqlContext(this IServiceCollection services,
IConfiguration configuration) =>
services.AddDbContext<RepositoryContext>(opts =>
opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
    public static void ConfigureResponseCaching(this IServiceCollection services) =>
services.AddResponseCaching();
    public static void ConfigureHttpCacheHeaders(this IServiceCollection services) =>
services.AddHttpCacheHeaders((expirationOpt) =>
    {
        expirationOpt.MaxAge = 65;
        //expirationOpt.CacheLocation = CacheLocation.Private;
    },
    (validationOpt) =>
    {
        validationOpt.MustRevalidate = true;
    });
    //public static void ConfigureIdentity(this IServiceCollection services)
    //{
    //    var builder = services.AddIdentity<User, IdentityRole>(o =>
    //    {
    //        o.Password.RequireDigit = true;
    //        o.Password.RequireLowercase = false;
    //        o.Password.RequireUppercase = false;
    //        o.Password.RequireNonAlphanumeric = false;
    //        o.Password.RequiredLength = 8;
    //        o.User.RequireUniqueEmail = true;
    //    })
    //    .AddEntityFrameworkStores<RepositoryContext>()
    //    .AddDefaultTokenProviders();
    //}
//    public static void ConfigureJWT(this IServiceCollection services, IConfiguration
//configuration)
//    {
//        var jwtSettings = configuration.GetSection("JwtSettings");
//        services.AddAuthentication(opt =>
//        {
//            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//        })
//        .AddJwtBearer(options =>
//        {
//            options.TokenValidationParameters = new TokenValidationParameters
//            {
//                ValidateIssuer = true,
//                ValidateAudience = true,
//                ValidateLifetime = true,
//                ValidateIssuerSigningKey = true,
//                ValidIssuer = jwtSettings["validIssuer"],
//                ValidAudience = jwtSettings["validAudience"],
//                IssuerSigningKey = new
//    SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["validKey"]!))
//            };
//        });
//    }




}
