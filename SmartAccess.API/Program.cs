using Microsoft.EntityFrameworkCore;
using SmartAccess.Application.Contracts;
using SmartAccess.Application.UseCases;
using SmartAccess.Domain.Repositories;
using SmartAccess.Infrastructure.Persistence;
using SmartAccess.Infrastructure.Repositories;
using FluentValidation;
using SmartAccess.API.Validators;
using SmartAccess.Application.DTOs;
using SmartAccess.Application.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<SmartAccessDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
builder.Services.AddScoped<IGetUserByIdUseCase, GetUserByIdUseCase>();
builder.Services.AddScoped<IGetAllUsersUseCase, GetAllUsersUseCase>();
builder.Services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
builder.Services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
builder.Services.AddScoped<ISearchUsersUseCase, SearchUsersUseCase>();
builder.Services.AddScoped<ISetUserStatusUseCase, SetUserStatusUseCase>();


// Controller & swagger
builder.Services.AddControllers();
builder.Services.AddScoped<IValidator<UserDto>, UserDtoValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Urls.Clear();
app.Urls.Add("http://0.0.0.0:8080");

builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

app.Run();
