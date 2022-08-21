namespace Moby.Services.Identity.DbInitializer;

public interface IDbInitializer
{
    Task InitializeAsync();
}