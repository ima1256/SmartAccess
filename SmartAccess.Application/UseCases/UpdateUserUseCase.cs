using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using SmartAccess.Application.Contracts;
using SmartAccess.Application.DTOs;
using SmartAccess.Domain.Entities;
using SmartAccess.Domain.Repositories;

namespace SmartAccess.Application.UseCases
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {

        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UpdateUserUseCase(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UpdateUserResponse> Execute(Guid id, UserDto dto)
        {

            if (dto is null) return new UpdateUserResponse { Result = UpdateUserResult.InvalidInput };
            var user = await _repo.GetByIdAsync(id);

            if (user is null) return new UpdateUserResponse { Result = UpdateUserResult.NotFound };

            _mapper.Map(dto, user);

            await _repo.UpdateAsync(user);

            var updatedUser = _mapper.Map<UserDto>(user);

            return new UpdateUserResponse { Result = UpdateUserResult.Success, UpdatedUser = updatedUser };
        }
    }
}