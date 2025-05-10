using BookAPI.Repositories;

namespace BookAPI.Startup;

public static class RepositoriesConfig
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IBooksRepository, BooksRepository>();
        services.AddTransient<IAuthorsRepository, AuthorsRepository>();
        services.AddTransient<IAuthorBooksRepository, AuthorBooksRepository>();
    }
}
