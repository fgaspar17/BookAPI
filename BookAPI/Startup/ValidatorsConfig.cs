using BookAPI.DTOs.RequestDTOs;
using BookAPI.Validators;
using FluentValidation;

namespace BookAPI.Startup;

public static class ValidatorsConfig
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<AuthorPostRequestDto>, AuthorPostRequestDtoValidator>();
        services.AddScoped<IValidator<AuthorPutRequestDto>, AuthorPutRequestDtoValidator>();
        services.AddScoped<IValidator<BookPostRequestDto>, BookPostRequestDtoValidator>();
        services.AddScoped<IValidator<BookPutRequestDto>, BookPutRequestDtoValidator>();
        services.AddScoped<IValidator<AuthorBookPostRequestDto>, AuthorBookPostRequestDtoValidator>();
        services.AddScoped<AuthorQueryParametersValidator>();
        services.AddScoped<BookQueryParametersValidator>();
    }
}
