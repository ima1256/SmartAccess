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

        public UsersController(RegisterUserUseCase registerUser, GetUserByIdUseCase getUserById)
        {
            _registerUser = registerUser;
            _getUserById = getUserById;
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
    }
}