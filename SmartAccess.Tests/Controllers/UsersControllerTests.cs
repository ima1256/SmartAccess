using Moq;
using SmartAccess.API.Controllers;
using SmartAccess.Application.DTOs;
using SmartAccess.Application.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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


    private readonly Mock<ISetUserStatusUseCase> _setUserStatus = new();

    public UsersControllerTests()
    {
        _controller = new UsersController(
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

        result.Should().BeOfType<BadRequestResult>();
        _registerUser.Verify(u => u.Execute(It.IsAny<UserDto>()), Times.Never);

    }

    [Fact]
    public async Task Register_EmptyUsername_ReturnsBadRequest()
    {
        var dto = new UserDto { Username = "", RFIDCard = "RFID123", IsActive = true };

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


}

