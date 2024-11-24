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
public class LocalityControllerTests
{
    private readonly Mock<IServiceManager> _serviceMock;
    private readonly LocalityController _controller;

    public LocalityControllerTests()
    {
        _serviceMock = new Mock<IServiceManager>();
        _controller = new LocalityController(_serviceMock.Object);
    }

    [Fact]
    public void Index_ReturnsOkResult_WithAllLocalities()
    {
        // Arrange
        var localities = new List<Locality>
        {
            new Locality { Id = 1, Name = "Locality1" },
            new Locality { Id = 2, Name = "Locality2" }
        };
        _serviceMock.Setup(s => s.LocalityService.GetAllLocalities(false)).Returns(localities);

        // Act
        var result = _controller.Index();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<List<Locality>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public void Details_ReturnsOkResult_WithExistingLocality()
    {
        // Arrange
        var locality = new Locality { Id = 1, Name = "Locality1" };
        _serviceMock.Setup(s => s.LocalityService.GetLocality(1)).Returns(locality);

        // Act
        var result = _controller.Details(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<Locality>(okResult.Value);
        Assert.Equal(locality.Id, returnValue.Id);
    }

    [Fact]
    public void Details_ReturnsNotFound_WhenLocalityDoesNotExist()
    {
        // Arrange
        _serviceMock.Setup(s => s.LocalityService.GetLocality(1)).Returns((Locality)null);

        // Act
        var result = _controller.Details(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_ReturnsNoContent_WhenLocalityExists()
    {
        // Arrange
        var locality = new Locality { Id = 1, Name = "Locality1" };
        _serviceMock.Setup(s => s.LocalityService.GetLocality(1)).Returns(locality);

        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _serviceMock.Verify(s => s.LocalityService.Remove(locality), Times.Once);
    }

    [Fact]
    public void Edit_ReturnsOkResult_WhenValidInput()
    {
        // Arrange
        var localityDTO = new LocalityEditDTO { Id = 1, Name = "UpdatedLocality" };
        var locality = new Locality { Id = localityDTO.Id, Name = localityDTO.Name };

        _serviceMock.Setup(s => s.LocalityService.Edit(It.IsAny<Locality>()));

        // Act
        var result = _controller.Edit(localityDTO);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<Locality>(okResult.Value);
        Assert.Equal(locality.Id, returnValue.Id);
        Assert.Equal(locality.Name, returnValue.Name);
    }

    [Fact]
    public void Create_ReturnsOkResult_WhenValidInput()
    {
        // Arrange
        var localityDTO = new LocalityEditDTO { Id = 2, Name = "NewLocality" };
        var locality = new Locality { Id = localityDTO.Id, Name = localityDTO.Name };

        _serviceMock.Setup(s => s.LocalityService.Add(It.IsAny<Locality>()));

        // Act
        var result = _controller.Create(localityDTO);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<Locality>(okResult.Value);
        Assert.Equal(locality.Id, returnValue.Id);
        Assert.Equal(locality.Name, returnValue.Name);
    }

    [Fact]
    public void ForSelect_ReturnsSelectList()
    {
        // Arrange
        var localities = new List<Locality>
        {
            new Locality { Id = 1, Name = "Locality1" },
            new Locality { Id = 2, Name = "Locality2" }
        };
        _serviceMock.Setup(s => s.LocalityService.GetAllLocalities(false)).Returns(localities);

        // Act
        var result = _controller.ForSelect();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var selectList = Assert.IsAssignableFrom<SelectList>(okResult.Value);
        Assert.Equal(2, selectList.Count());
    }
}