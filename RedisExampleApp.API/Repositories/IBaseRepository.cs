namespace RedisExampleApp.API.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<T> CreateAsync(T entity);
    }
}
