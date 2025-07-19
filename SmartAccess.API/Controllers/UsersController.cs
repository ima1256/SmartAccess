using Microsoft.AspNetCore.Mvc;
using SmartAccess.Application.DTOs;
using SmartAccess.Application.UseCases;
using SmartAccess.Domain.Entities;

namespace SmartAccess.API.Controllers
{



    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly RegisterUserUseCase _registerUser;
        private readonly GetUserByIdUseCase _getUserById;
        private readonly GetAllUsersUseCase _getAllUsers;
        private readonly UpdateUserUseCase _updateUser;
        private readonly DeleteUserUseCase _deleteUser;
        private readonly SearchUsersUseCase _searchUsers;
        private readonly SetUserStatusUseCase _setUserStatus;

        public UsersController(SetUserStatusUseCase setUserStatus, SearchUsersUseCase searchUsers, DeleteUserUseCase deleteUser, UpdateUserUseCase updateUser, RegisterUserUseCase registerUser, GetUserByIdUseCase getUserById, GetAllUsersUseCase getAllUsers)
        {
            _registerUser = registerUser;
            _getUserById = getUserById;
            _getAllUsers = getAllUsers;
            _updateUser = updateUser;
            _deleteUser = deleteUser;
            _searchUsers = searchUsers;
            _setUserStatus = setUserStatus;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto dto)
        {
            await _registerUser.Execute(dto);
            return Ok("User registered succesfully.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _getUserById.Execute(id);
            return user is not null ? Ok(user) : NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _getAllUsers.Execute();
            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UserDto dto)
        {
            var result = await _updateUser.Execute(id, dto);
            return result ? Ok("User updated.") : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _deleteUser.Execute(id);
            return result ? Ok("User deleted.") : NotFound();
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string text, [FromQuery] bool? status)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (text is null && status is null)
            {
                Console.WriteLine("These are null");
            }
            Console.WriteLine(text);
            Console.ResetColor();
            var users = await _searchUsers.Execute(text, status);
            return Ok(users);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> SetStatus(Guid id, [FromQuery] bool status)
        {

            var result = await _setUserStatus.Execute(id, status);
            return result ? Ok("Status changed.") : NotFound();
        }
    }
}