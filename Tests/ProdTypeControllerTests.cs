using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using InnoShop.DTO.Models;
using InnoShop.ProdWebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests;
public class ProdTypeControllerTests
{
    private readonly Mock<IServiceManager> _serviceMock;
    private readonly ProdTypeController _controller;

    public ProdTypeControllerTests()
    {
        _serviceMock = new Mock<IServiceManager>();
        _controller = new ProdTypeController(_serviceMock.Object);
    }

    [Fact]
    public void Index_ReturnsOkResult_WithAllProdTypes()
    {
        // Arrange
        var prodTypes = new List<ProdType>
        {
            new ProdType { Id = 1, Name = "Type1" },
            new ProdType { Id = 2, Name = "Type2" }
        };
        _serviceMock.Setup(s => s.ProdTypeService.GetAllProdTypes(false)).Returns(prodTypes);

        // Act
        var result = _controller.Index();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<List<ProdType>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public void Details_ReturnsOkResult_WithExistingProdType()
    {
        // Arrange
        var prodType = new ProdType { Id = 1, Name = "Type1" };
        _serviceMock.Setup(s => s.ProdTypeService.GetProdType(1)).Returns(prodType);

        // Act
        var result = _controller.Details(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<ProdType>(okResult.Value);
        Assert.Equal(prodType.Id, returnValue.Id);
    }

    [Fact]
    public void Details_ReturnsNotFound_WhenProdTypeDoesNotExist()
    {
        // Arrange
        _serviceMock.Setup(s => s.ProdTypeService.GetProdType(1)).Returns((ProdType)null);

        // Act
        var result = _controller.Details(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Edit_ReturnsOkResult_WhenValidInput()
    {
        // Arrange
        var localityDTO = new LocalityEditDTO { Id = 1, Name = "UpdatedType" };
        var locality = new ProdType { Id = localityDTO.Id, Name = localityDTO.Name };

        _serviceMock.Setup(s => s.ProdTypeService.Edit(It.IsAny<ProdType>()));

        // Act
        var result = _controller.Edit(localityDTO);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<ProdType>(okResult.Value);
        Assert.Equal(locality.Id, returnValue.Id);
        Assert.Equal(locality.Name, returnValue.Name);
    }

    [Fact]
    public void Create_ReturnsOkResult_WhenValidInput()
    {
        // Arrange
        var localityDTO = new LocalityEditDTO { Id = 2, Name = "NewType" };
        var locality = new ProdType { Id = localityDTO.Id, Name = localityDTO.Name };

        _serviceMock.Setup(s => s.ProdTypeService.Add(It.IsAny<ProdType>()));

        // Act
        var result = _controller.Create(localityDTO);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<ProdType>(okResult.Value);
        Assert.Equal(locality.Id, returnValue.Id);
        Assert.Equal(locality.Name, returnValue.Name);
    }

    [Fact]
    public void Delete_ReturnsNoContent_WhenProdTypeExists()
    {
        // Arrange
        var prodType = new ProdType { Id = 1, Name = "Type1" };
        _serviceMock.Setup(s => s.ProdTypeService.GetProdType(1)).Returns(prodType);

        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _serviceMock.Verify(s => s.ProdTypeService.Remove(prodType), Times.Once);
    }

    [Fact]
    public void ForSelect_ReturnsSelectList()
    {
        // Arrange
        var prodTypes = new List<ProdType>
        {
            new ProdType { Id = 1, Name = "Type1" },
            new ProdType { Id = 2, Name = "Type2" }
        };
        _serviceMock.Setup(s => s.ProdTypeService.GetAllProdTypes(false)).Returns(prodTypes);

        // Act
        var result = _controller.ForSelect();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var selectList = Assert.IsAssignableFrom<SelectList>(okResult.Value);
        Assert.Equal(2, selectList.Count());
    }
}
