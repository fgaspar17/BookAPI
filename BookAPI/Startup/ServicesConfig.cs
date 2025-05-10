using BookAPI.Services;

namespace BookAPI.Startup;

public static class ServicesConfig
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthorsService, AuthorsService>();
        services.AddTransient<IBooksService, BooksService>();
        services.AddTransient<IAuthorBooksService, AuthorBooksService>();
    }
}
