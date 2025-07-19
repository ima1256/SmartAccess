using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SmartAccess.Application.Contracts;
using SmartAccess.Application.DTOs;
using SmartAccess.Domain.Entities;
using SmartAccess.Domain.Repositories;

namespace SmartAccess.Application.UseCases
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {
        private readonly IUserRepository _repo;

        public UpdateUserUseCase(IUserRepository repo) => _repo = repo;

        public async Task<bool> Execute(Guid id, UserDto dto)
        {

            var user = await _repo.GetByIdAsync(id);

            if (user is null) return false;

            user.Username = dto.Username;
            user.RFIDCard = dto.RFIDCard;

            await _repo.UpdateAsync(user);

            return true;
        }
    }
}