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
using SmartAccess.Tests.Helpers;
using SmartAccess.Application.UseCases;

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

    [Fact]
    public async Task Get_MapsUserToUserDto_ReturnsExpectedDto()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Username = "imanol",
            RFIDCard = "RFID999",
            IsActive = true,
            Role = "Admin"
        };

        var expectedDto = new UserDto
        {
            Id = userId.ToString(),
            Username = "imanol",
            RFIDCard = "RFID999",
            IsActive = true
        };

        _getUserById.Setup(s => s.Execute(userId)).ReturnsAsync(user);

        var result = await _controller.Get(userId);

        result.Should().BeOfType<OkObjectResult>();
        var ok = result as OkObjectResult;
        ok!.Value.Should().BeEquivalentTo(expectedDto, options => options.ExcludingMissingMembers());

        _getUserById.Verify(s => s.Execute(userId), Times.Once);

    }

    #endregion

    #region GetAll

    [Fact]
    public async Task GetAll_NoUsers_ReturnEmptyList()
    {
        _getAllUsers.Setup(s => s.Execute()).ReturnsAsync(Enumerable.Empty<User>());

        var result = await _controller.GetAll();

        result.Should().BeOfType<OkObjectResult>();
        var okResult = Assert.IsType<OkObjectResult>(result);
        var users = Assert.IsAssignableFrom<IEnumerable<UserDto>>(okResult.Value);
        users.Should().BeEmpty();
        _logger.VerifyLog(LogLevel.Information, "No users found");

    }

    [Fact]
    public async Task GetAll_UsersExist_ReturnsList()
    {
        var users = new List<User> {
                new () { Id = Guid.NewGuid(), Username = "User1", RFIDCard = "RFID1", IsActive = true },
                new () { Id = Guid.NewGuid(), Username = "User2", RFIDCard = "RFID2", IsActive = false },
            };

        _getAllUsers.Setup(s => s.Execute()).ReturnsAsync(users);

        var result = await _controller.GetAll();

        result.Should().BeOfType<OkObjectResult>();

        var okResult = Assert.IsType<OkObjectResult>(result);

        var userDtos = Assert.IsAssignableFrom<IEnumerable<UserDto>>(okResult.Value);
        userDtos.Should().BeEquivalentTo(users.Select(u => u.ToDto(_mapper)));
    }

    [Fact]
    public async Task GetAll_ThrowsException_Returns500()
    {
        _getAllUsers.Setup(u => u.Execute()).ThrowsAsync(new Exception("Server error"));

        var result = await _controller.GetAll();

        var r = result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);

        _logger.VerifyLog(LogLevel.Error, "error occurred");

    }

    #endregion

    #region UpdateUser

    [Fact]
    public async Task UpdateUser_InvalidInput_ReturnsBadRequest()
    {
        var userId = Guid.NewGuid();

        UserDto? userDto = null;

        var result = await _controller.UpdateUser(userId, userDto);

        result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().Be("User data must be provided.");


    }

    [Fact]
    public async Task UpdateUser_UserNotFound_ReturnsNotFound()
    {
        var userId = Guid.NewGuid();
        var dto = new UserDto { Id = userId.ToString(), Username = "test", RFIDCard = "RFID123", IsActive = true };
        _updateUser.Setup(s => s.Execute(userId, dto)).ReturnsAsync(new UpdateUserResponse { Result = UpdateUserResult.NotFound, UpdatedUser = null });

        var result = await _controller.UpdateUser(userId, dto);

        result.Should().BeOfType<NotFoundObjectResult>().Which.Value.Should().Be($"User with ID {userId} not found.");

    }

    [Fact]
    public async Task UpdateUser_Success_ReturnsOkWithUpdatedUser()
    {
        var userId = Guid.NewGuid();
        var dto = new UserDto { Id = userId.ToString(), Username = "test", RFIDCard = "RFID123", IsActive = true };
        _updateUser.Setup(s => s.Execute(userId, dto)).ReturnsAsync(new UpdateUserResponse
        {
            Result = UpdateUserResult.Success,
            UpdatedUser = dto
        });

        var result = await _controller.UpdateUser(userId, dto);

        var okResult = result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(dto);

    }

    [Fact]
    public async Task UpdateUser_UnexpectedResult_ReturnsInternalServerError()
    {
        var userId = Guid.NewGuid();
        var dto = new UserDto { Id = userId.ToString(), Username = "test", RFIDCard = "RFID123", IsActive = true };
        _updateUser.Setup(s => s.Execute(userId, dto)).ReturnsAsync(new UpdateUserResponse
        {
            Result = (UpdateUserResult)999,
            UpdatedUser = null
        });
        var result = await _controller.UpdateUser(userId, dto);
        var objectResult = result.Should().BeOfType<ObjectResult>().Which;
        objectResult.StatusCode.Should().Be(500);
        objectResult.Value.Should().Be("Unexpected error.");
    }

    [Fact]
    public async Task UpdateUser_ExceptionThrown_ReturnsInternalServerError()
    {
        var userId = Guid.NewGuid();
        var dto = new UserDto { Id = userId.ToString(), Username = "test", RFIDCard = "RFID123", IsActive = true };
        _updateUser.Setup(s => s.Execute(userId, dto)).ThrowsAsync(new Exception("An internal server error occurred."));
        var result = await _controller.UpdateUser(userId, dto);
        var objectResult = result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
        _logger.VerifyLog(LogLevel.Error, $"An error occurred while updating user with ID: {userId}.");
    }

    #endregion

}

