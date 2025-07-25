using Microsoft.AspNetCore.Mvc;
using SmartAccess.Application.DTOs;
using SmartAccess.Application.Contracts;
using SmartAccess.Domain.Entities;
using Microsoft.Extensions.Logging;
using AutoMapper;
using SmartAccess.Application.Mapping;

namespace SmartAccess.API.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;

        private readonly IRegisterUserUseCase _registerUser;
        private readonly IGetUserByIdUseCase _getUserById;
        private readonly IGetAllUsersUseCase _getAllUsers;
        private readonly IUpdateUserUseCase _updateUser;
        private readonly IDeleteUserUseCase _deleteUser;
        private readonly ISearchUsersUseCase _searchUsers;
        private readonly ISetUserStatusUseCase _setUserStatus;

        public UsersController(
            IMapper mapper,
            ILogger<UsersController> logger,
            ISetUserStatusUseCase setUserStatus,
            ISearchUsersUseCase searchUsers,
            IDeleteUserUseCase deleteUser,
            IUpdateUserUseCase updateUser,
            IRegisterUserUseCase registerUser,
            IGetUserByIdUseCase getUserById,
            IGetAllUsersUseCase getAllUsers)
        {
            _mapper = mapper;
            _logger = logger;
            _registerUser = registerUser;
            _getUserById = getUserById;
            _getAllUsers = getAllUsers;
            _updateUser = updateUser;
            _deleteUser = deleteUser;
            _searchUsers = searchUsers;
            _setUserStatus = setUserStatus;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto? dto)
        {

            if (dto == null)
            {
                return BadRequest("Request body is null.");
            }

            if (dto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _registerUser.Execute(dto);
                return Ok("User registered successfully.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error registering user with Username: {Username}", dto.Username);
                return StatusCode(500, "An internal server error ocurred.");
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {

            if (id == Guid.Empty)
            {
                return BadRequest("The provided user ID is invalid.");
            }

            try
            {
                var user = await _getUserById.Execute(id);

                if (user == null)
                {
                    _logger.LogWarning("User not found. ID: {UserId}", id);
                    return NotFound($"User with ID {id} was not found.");
                }

                return Ok(user.ToDto(_mapper));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with ID: {UserId}", id);
                return StatusCode(500, "An internal server error ocurred.");
            }
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