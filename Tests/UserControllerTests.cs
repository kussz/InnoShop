using Moq;
using Xunit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InnoShop.UserWebAPI.Controllers;
using InnoShop.Contracts.Service;
using InnoShop.Domain.Models;
using InnoShop.DTO.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using System.Data;

namespace Tests;
public class UserControllerTests
{
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<RoleManager<IdentityRole<int>>> _mockRoleManager;
    private readonly Mock<SignInManager<User>> _mockSignInManager;
    private readonly Mock<IServiceManager> _mockServiceManager;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IOptions<JwtSettings>> _mockJwtSettings;
    private readonly UserController _controller;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly Mock<IUserClaimsPrincipalFactory<User>> _mockClaimsFactory;
    private readonly Mock<IOptions<IdentityOptions>> _mockIdentityOptions;
    private readonly Mock<ILogger<SignInManager<User>>> _mockLogger;
    private readonly Mock<IAuthenticationSchemeProvider> _mockAuthenticationSchemeProvider;
    private readonly Mock<IUserConfirmation<User>> _mockUserConfirmation;
    private readonly ITestOutputHelper _output;
    private readonly string _token;
    private readonly int _actualId;

    public UserControllerTests(ITestOutputHelper output)
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _output = output;
        var roleStoreMock = new Mock<IRoleStore<IdentityRole<int>>>();
        _mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        _mockRoleManager = new Mock<RoleManager<IdentityRole<int>>>(roleStoreMock.Object, null, null, null, null);
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockClaimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
        _mockJwtSettings = new Mock<IOptions<JwtSettings>>();
        _mockIdentityOptions= new Mock<IOptions<IdentityOptions>>();
        _mockLogger= new Mock<ILogger<SignInManager<User>>>();
        _mockAuthenticationSchemeProvider= new Mock<IAuthenticationSchemeProvider>();
        _mockUserConfirmation= new Mock<IUserConfirmation<User>>();
        _mockSignInManager = new Mock<SignInManager<User>>(_mockUserManager.Object, _mockHttpContextAccessor.Object, _mockClaimsFactory.Object, _mockIdentityOptions.Object, _mockLogger.Object, _mockAuthenticationSchemeProvider.Object, _mockUserConfirmation.Object);
        _mockServiceManager = new Mock<IServiceManager>();
        _mockConfiguration = new Mock<IConfiguration>();
        var mockUserService = new Mock<IUserService>();
        _mockServiceManager.Setup(service => service.UserService).Returns(mockUserService.Object);
        _token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJwZXBlIiwianRpIjoiZDI4NGMzM2QtMWY4Yy00YzIxLWE0OWEtNTdkMTA1ZTM1Mjk5IiwiSWQiOiIxOTI2Iiwicm9sZSI6IkFkbWluIiwibmJmIjoxNzMyMjYyNTQ1LCJleHAiOjE3MzIyNjQzNDUsImlzcyI6Iklubm9TaG9wIiwiYXVkIjoiUGVvcGxlIn0.mOxCQKdAlhIiBGFPbW-YaJdoOP_ybL9wGHTTN31A32U";
        _actualId = 1926;

        // Установите значения для JwtSettings
        _mockJwtSettings.Setup(m => m.Value).Returns(new JwtSettings { SecretKey = "DefinetelyNotACommonSecretKeyForSecurityTokens",
            Issuer = "InnoShop", Audience = "People"
        });
        _controller = new UserController(_mockUserManager.Object, _mockRoleManager.Object, _mockSignInManager.Object, _mockServiceManager.Object, _mockJwtSettings.Object, _mockConfiguration.Object);

    }
    private void SetAuth(string token)
    {
        _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        _controller.ControllerContext.HttpContext.Request.Headers.Authorization = $"Bearer {token}";
    }
    [Fact]
    public async Task Login_ReturnsOkResult_WithValidCredentials()
    {
        // Arrange
        var model = new UserLoginDTO { UserName = "testuser", Password = "password" };
        var user = new User { UserName = "testuser", Id = 1 };
        var roles = new string[] { "Admin" };

        _mockUserManager.Setup(x => x.FindByNameAsync(model.UserName)).ReturnsAsync(user);
        _mockUserManager.Setup(x => x.CheckPasswordAsync(user, model.Password)).ReturnsAsync(true);
        _mockUserManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(roles);
        

        // Act
        var result = await _controller.Login(model);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        _output.WriteLine(Assert.IsType<string>(okResult.Value)); // Проверка, что возвращается токен
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WithInvalidCredentials()
    {
        // Arrange
        var model = new UserLoginDTO { UserName = "testuser", Password = "wrongpassword" };
        var user = new User { UserName = "testuser", Id = 1 };

        _mockUserManager.Setup(x => x.FindByNameAsync(model.UserName)).ReturnsAsync(user);
        _mockUserManager.Setup(x => x.CheckPasswordAsync(user, model.Password)).ReturnsAsync(false);

        // Act
        var result = await _controller.Login(model);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task Register_ReturnsOkResult_WhenUserIsCreated()
    {
        // Arrange
        var model = new UserRegisterDTO
        {
            UserName = "newuser",
            Email = "newuser@example.com",
            Password = "password",
            PasswordConfirm = "password",
            UserTypeId = 1,
            LocalityId = 1
        };
        string[] roles = ["User"];
        var user = new User { UserName = model.UserName, Email = model.Email, Id = 1 };
        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), model.Password)).ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(x => x.FindByNameAsync(model.UserName)).ReturnsAsync(user);
        _mockServiceManager.Setup(s => s.UserService.GetUser(user.Id)).Returns(user);
        _mockRoleManager.Setup(r => r.RoleExistsAsync("User")).ReturnsAsync(true);
        _mockUserManager.Setup(u => u.AddToRoleAsync(user, "User")).ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);

        // Act
        var result = await _controller.Register(model);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<string>(okResult.Value); // Проверка, что возвращается токен
    }

    [Fact]
    public async Task CreateRole_ReturnsOkResult_WhenRoleIsCreated()
    {
        // Arrange
        var roleName = "NewRole";
        _mockRoleManager.Setup(x => x.RoleExistsAsync(roleName)).ReturnsAsync(false);
        _mockRoleManager.Setup(x => x.CreateAsync(It.IsAny<IdentityRole<int>>())).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _controller.CreateRole(roleName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)okResult.Value); // Проверка, что роль была создана
    }

    [Fact]
    public void GetProfile_ReturnsOkResult_WhenUserIsAuthorized()
    {
        // Arrange
        var user = new User { Id = _actualId, UserName = "testuser" };
        SetAuth(_token);
        _mockServiceManager.Setup(service => service.UserService.Authorize(It.IsAny<string>())).Returns(user);
        // Act
        var result = _controller.GetProfile();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(user, okResult.Value); // Проверка, что возвращается пользователь
    }

    [Fact]
    public void GetProfile_ReturnsUnauthorized_WhenUserIsNotAuthorized()
    {
        SetAuth("");
        // Arrange
        _mockServiceManager.Setup(service => service.UserService.Authorize(It.IsAny<string>())).Returns((User)null);

        // Act
        var result = _controller.GetProfile();

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }
}