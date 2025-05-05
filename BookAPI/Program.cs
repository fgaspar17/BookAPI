using BookAPI.Models;
using BookAPI.Repositories;
using BookAPI.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
 options.UseSqlite(
 builder.Configuration.GetConnectionString("DefaultConnection"))
 );

builder.Services.AddTransient<IBooksRepository, BooksRepository>();
builder.Services.AddTransient<IAuthorsRepository, AuthorsRepository>();
builder.Services.AddTransient<IAuthorBooksRepository, AuthorBooksRepository>();

builder.Services.AddTransient<IAuthorsService, AuthorsService>();
builder.Services.AddTransient<IBooksService, BooksService>();
builder.Services.AddTransient<IAuthorBooksService, AuthorBooksService>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Books API")
            .WithTheme(ScalarTheme.Alternate)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
