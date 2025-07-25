using Moq;
using SmartAccess.API.Controllers;
using SmartAccess.Application.DTOs;
using SmartAccess.Application.Contracts;
using SmartAccess.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartAccess.Application.Mapping;
using AutoMapper;

public class UsersControllerTests
{
    private readonly UsersController _controller;

    private readonly Mock<IRegisterUserUseCase> _registerUser = new();
    private readonly Mock<IGetUserByIdUseCase> _getUserById = new();
    private readonly Mock<IGetAllUsersUseCase> _getAllUsers = new();
    private readonly Mock<IUpdateUserUseCase> _updateUser = new();
    private readonly Mock<IDeleteUserUseCase> _deleteUser = new();
    private readonly Mock<ISearchUsersUseCase> _searchUsers = new();
    private readonly Mock<ILogger<UsersController>> _logger = new();
    private readonly IMapper _mapper;


    private readonly Mock<ISetUserStatusUseCase> _setUserStatus = new();

    public UsersControllerTests()
    {

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserProfile>(); // Tu perfil de mapeo
        });
        _mapper = config.CreateMapper();

        _controller = new UsersController(
            _mapper,
            _logger.Object,
            _setUserStatus.Object,
            _searchUsers.Object,
            _deleteUser.Object,
            _updateUser.Object,
            _registerUser.Object,
            _getUserById.Object,
            _getAllUsers.Object
        );
    }

    #region Register

    [Fact]
    public async Task Register_ReturnsOkResult()
    {
        var dto = new UserDto { Username = "Imanol", RFIDCard = "34S33F", IsActive = false };

        var result = await _controller.Register(dto);

        _registerUser.Verify(u => u.Execute(dto), Times.Once);

    }

    [Fact]
    public async Task Register_NullDto_ReturnsBadRequest()
    {
        var result = await _controller.Register(null);

        result.Should().BeOfType<BadRequestObjectResult>();
        _registerUser.Verify(u => u.Execute(It.IsAny<UserDto>()), Times.Never);

    }

    [Fact]
    public async Task Register_EmptyUsername_ReturnsBadRequest()
    {
        var dto = new UserDto { Username = "", RFIDCard = "RFID123", IsActive = true };

        _controller.ModelState.AddModelError("Username", "Username is required");

        var result = await _controller.Register(dto);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Register_ThrowsException_Returns500()
    {
        var dto = new UserDto { Username = "ErrorUser", RFIDCard = "XXXXX", IsActive = false };
        _registerUser.Setup(u => u.Execute(dto)).ThrowsAsync(new Exception("Database error"));

        var result = await _controller.Register(dto);

        result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
    }

    #endregion

    #region Get

    [Fact]
    public async Task Get_UserExists_ReturnsOk()
    {
        var userId = Guid.NewGuid();

        var expectedUser = new UserDto
        {
            Id = userId.ToString(),
            Username = "testuser",
            RFIDCard = "RFID123",
            IsActive = true
        };

        _getUserById.Setup(s => s.Execute(userId)).ReturnsAsync(expectedUser.ToEntity(_mapper));

        var result = await _controller.Get(userId);

        result.Should().BeOfType<OkObjectResult>();

        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedUser);

        _getUserById.Verify(s => s.Execute(userId), Times.Once);

    }

    [Fact]
    public async Task Get_UserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _getUserById.Setup(s => s.Execute(userId)).ReturnsAsync((User?)null); // Simula que no lo encuentra

        // Act
        var result = await _controller.Get(userId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();

        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult!.Value.Should().Be($"User with ID {userId} was not found.");

        _getUserById.Verify(s => s.Execute(userId), Times.Once);
    }

    [Fact]
    public async Task Get_EmptyGuid_ReturnsBadRequest()
    {
        var invalidId = Guid.Empty;
        var result = await _controller.Get(invalidId);

        result.Should().BeOfType<BadRequestObjectResult>();

        var badRequest = result as BadRequestObjectResult;

        badRequest!.Value.Should().Be("The provided user ID is invalid.");
        _getUserById.Verify(s => s.Execute(It.IsAny<Guid>()), Times.Never);

    }

    [Fact]
    public async Task Get_ExceptionThrown_ReturnsInternalServerError()
    {
        var userId = Guid.NewGuid();
        _getUserById.Setup(s => s.Execute(userId)).ThrowsAsync(new Exception("Something went wrong"));

        var result = await _controller.Get(userId);

        result.Should().BeOfType<ObjectResult>();

        var objectResult = result as ObjectResult;

        objectResult!.StatusCode.Should().Be(500);

        objectResult.Value.Should().Be("An internal server error ocurred.");

        _getUserById.Verify(s => s.Execute(userId), Times.Once);

    }

    #endregion

}

