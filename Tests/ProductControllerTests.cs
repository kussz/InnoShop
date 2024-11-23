using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using InnoShop.DTO.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using InnoShop.ProdWebAPI.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Microsoft.AspNetCore.Http;
using Xunit.Abstractions;
using NuGet.Common;
using Azure.Core;
namespace Tests;

public class ProductControllerTests
{
    private readonly Mock<IServiceManager> _mockServiceManager;
    private readonly ProductController _controller;
    private readonly string _token;
    private readonly int _actualId;
    private readonly ITestOutputHelper _output;

    public ProductControllerTests(ITestOutputHelper output)
    {
        _mockServiceManager = new Mock<IServiceManager>();
        var mockProductService = new Mock<IProductService>();
        _controller = new ProductController(_mockServiceManager.Object);
        _mockServiceManager.Setup(service => service.ProductService).Returns(mockProductService.Object);
        _token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJwZXBlIiwianRpIjoiZDI4NGMzM2QtMWY4Yy00YzIxLWE0OWEtNTdkMTA1ZTM1Mjk5IiwiSWQiOiIxOTI2Iiwicm9sZSI6IkFkbWluIiwibmJmIjoxNzMyMjYyNTQ1LCJleHAiOjE3MzIyNjQzNDUsImlzcyI6Iklubm9TaG9wIiwiYXVkIjoiUGVvcGxlIn0.mOxCQKdAlhIiBGFPbW-YaJdoOP_ybL9wGHTTN31A32U";
        _actualId = 1926;
        _output = output;
    }
    private void SetAuth(string token)
    {
        _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        _controller.ControllerContext.HttpContext.Request.Headers.Authorization = $"Bearer {token}";
    }
    [Fact]
    public void Index_ReturnsOkResult_WithProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Test Product 1" },
            new() { Id = 2, Name = "Test Product 2" }
        };
        _mockServiceManager.Setup(service => service.ProductService.GetPage(30, 1)).Returns(products);

        // Act
        var result = _controller.Index(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnProducts = Assert.IsAssignableFrom<List<Product>>(okResult.Value);
        Assert.Equal(2, returnProducts.Count);
    }

    [Fact]
    public void ForUser_ReturnsOkResult_WithUserProducts()
    {
        // Arrange
        var user = new User { Id = _actualId };
        var products = new List<Product>
        {
            new (){ Id = 1, UserId = _actualId, Name = "User Product" }
        };
        SetAuth(_token);
        _mockServiceManager.Setup(service => service.UserService.Authorize(It.IsAny<string>())).Returns(user);
        _mockServiceManager.Setup(service => service.ProductService.GetProductsByCondition(It.IsAny<Expression<Func<Product, bool>>>(), false)).Returns(products);

        // Act
        var result = _controller.ForUser();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnProducts = Assert.IsAssignableFrom<List<Product>>(okResult.Value);
        Assert.Single(returnProducts);
    }

    [Fact]
    public void Create_ReturnsOkResult_WithProductEditData()
    {
        // Arrange
        var editData = new ProductEditData
        {
            Categories = [new ("Category1", "1")],
            Users = [new("User1", "1")]
        };
        _mockServiceManager.Setup(service => service.ProdTypeService.GetAllProdTypes(false)).Returns([new (){ Id = 1, Name = "Category1" }]);
        _mockServiceManager.Setup(service => service.UserService.GetAllUsers(false)).Returns([new (){ Id = 1, UserName = "User1" }]);

        // Act
        var result = _controller.Create();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnData = Assert.IsType<ProductEditData>(okResult.Value);
        Assert.Single(returnData.Categories);
        Assert.Single(returnData.Users);
    }

    [Fact]
    public void Create_Post_ReturnsOkResult_WhenProductIsCreated()
    {
        // Arrange
        var user = new User { Id = _actualId };
        var product = new Product { UserId = _actualId, Name = "New Product" };
        _output.WriteLine(user.ToString());
        SetAuth(_token);
        _mockServiceManager.Setup(service => service.UserService.Authorize(It.IsAny<string>())).Returns(user);

        // Act
        var result = _controller.Create(product);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnProduct = Assert.IsType<Product>(okResult.Value);
        Assert.Equal("New Product", returnProduct.Name);
    }

    [Fact]
    public void Create_Post_ReturnsBadRequest_WhenUserMismatch()
    {
        // Arrange
        var user = new User { Id = _actualId };
        var product = new Product { UserId = _actualId+1 }; // UserId не совпадает
        SetAuth(_token);
        _mockServiceManager.Setup(service => service.UserService.Authorize(It.IsAny<string>())).Returns(user);

        // Act
        var result = _controller.Create(product);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void Details_ReturnsOkResult_WithProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Test Product" };
        _mockServiceManager.Setup(service => service.ProductService.GetProduct(1)).Returns(product);

        // Act
        var result = _controller.Details(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnProduct = Assert.IsType<Product>(okResult.Value);
        Assert.Equal("Test Product", returnProduct.Name);
    }

    [Fact]
    public void Edit_Get_ReturnsOkResult_WithProductEditData()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Test Product" };
        _mockServiceManager.Setup(service => service.ProductService.GetProduct(1)).Returns(product);
        _mockServiceManager.Setup(service => service.ProdTypeService.GetAllProdTypes(false)).Returns([new (){ Id = 1, Name = "Category1" }]);
        _mockServiceManager.Setup(service => service.UserService.GetAllUsers(false)).Returns([new (){ Id = 1, UserName = "User1" }]);

        // Act
        var result = _controller.Edit(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnData = Assert.IsType<ProductEditData>(okResult.Value);
        Assert.NotNull(returnData.Product);
        Assert.Single(returnData.Categories);
        Assert.Single(returnData.Users);
    }

    [Fact]
    public void Edit_Post_ReturnsOkResult_WhenProductIsEdited()
    {
        _output.WriteLine("dskofskl");

        // Arrange
        var user = new User { Id = _actualId };
        string role = "Admin";
        var product = new Product { Id=1, UserId = _actualId, Name = "Edited Product" };
        SetAuth(_token);
        _mockServiceManager.Setup(service => service.UserService.GetRole($"Bearer {_token}")).Returns(role);
        _mockServiceManager.Setup(service => service.UserService.Authorize($"Bearer {_token}")).Returns(user) ;
        // Act
        var result = _controller.Edit(product);
        Console.Write(result.ToString());
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnProduct = Assert.IsType<Product>(okResult.Value);
        Assert.Equal("Edited Product", returnProduct.Name);
    }

    [Fact]
    public void Edit_Post_ReturnsBadRequest_WhenUserMismatch()
    {
        // Arrange
        var user = new User { Id = _actualId };
        var product = new Product { UserId = _actualId+1 }; // UserId не совпадает
        SetAuth(_token);
        _mockServiceManager.Setup(service => service.UserService.Authorize($"Bearer {_token}")).Returns(user);

        // Act
        var result = _controller.Edit(product);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void Delete_ReturnsNoContent_WhenProductIsDeleted()
    {
        // Arrange
        var user = new User { Id = _actualId };
        var product = new Product { Id = 1, UserId = _actualId };
        SetAuth(_token);
        _mockServiceManager.Setup(service => service.UserService.Authorize(It.IsAny<string>())).Returns(user);
        _mockServiceManager.Setup(service => service.ProductService.GetProduct(1)).Returns(product);

        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Delete_ReturnsBadRequest_WhenUserMismatch()
    {
        // Arrange
        var user = new User { Id = _actualId };
        var product = new Product { Id = 1, UserId = _actualId + 1 }; // UserId не совпадает
        SetAuth(_token);
        _mockServiceManager.Setup(service => service.UserService.Authorize(It.IsAny<string>())).Returns(user);
        _mockServiceManager.Setup(service => service.ProductService.GetProduct(1)).Returns(product);

        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
}