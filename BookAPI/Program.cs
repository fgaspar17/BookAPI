using BookAPI.DTOs.RequestDTOs;
using BookAPI.Models;
using BookAPI.Repositories;
using BookAPI.Services;
using BookAPI.Startup;
using BookAPI.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
 options.UseSqlite(
 builder.Configuration.GetConnectionString("DefaultConnection"))
 );

builder.Services.AddServices();

builder.Services.AddRepositories();

builder.Services.AddValidators();

builder.Services.AddControllers();

builder.Services.AddOpenApiServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
