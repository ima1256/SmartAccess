using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartAccess.Application.Contracts;
using SmartAccess.Application.DTOs;
using SmartAccess.Domain.Entities;
using SmartAccess.Domain.Repositories;

namespace SmartAccess.Application.UseCases
{
    public class SearchUsersUseCase : ISearchUsersUseCase
    {
        private readonly IUserRepository _repo;

        public SearchUsersUseCase(IUserRepository repo) => _repo = repo;

        public async Task<IEnumerable<UserDto?>> Execute(string? text, bool? isEnabled)
        {
            var users = await _repo.SearchAsync(text, isEnabled);

            return users.Select(u => new UserDto
            {
                Id = u!.Id.ToString(),
                Username = u.Username,
                RFIDCard = u.RFIDCard,
                IsActive = u.IsActive
            });
        }
    }
}