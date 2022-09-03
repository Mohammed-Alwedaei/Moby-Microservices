namespace Moby.Web.Client.Services.IServices;

public interface IBaseService : IDisposable
{
    Task<T> GetByIdAsync<T>(int id);

    Task<T> GetAllAsync<T>();

    Task<T> CreateByIdAsync<T>(T entityToCreate);

    Task<T> UpdateByIdAsync<T>(T entityToUpdate);

    Task DeleteAsync<T>(int id);
}