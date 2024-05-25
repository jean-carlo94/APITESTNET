using APITEST.Common.Interfaces;
using APITEST.Data;
using APITEST.Modules.Users.DTOs;
using APITEST.Modules.Users.Services;
using APITEST.Modules.Users.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database Conection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add services to the container.

//Validators
builder.Services.AddScoped<IValidator<UserInserDto>, UserInsertValidator>();
builder.Services.AddScoped<IValidator<UserUpdateDto>, UserUpdateValidator>();

//Services
builder.Services.AddKeyedScoped<ICommonService<UserDto, UserInserDto, UserUpdateDto>, UserService>("userService");

//Controller
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
