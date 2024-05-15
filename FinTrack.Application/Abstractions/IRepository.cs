namespace FinTrack.Application.Abstractions
{
    public interface IRepository<T>
    {
        Task<T> Add(T entity);
        Task<T> Delete(T enitity);
        Task<T> Update(int id, T entity);
        Task<T?> Get(int id);
        Task<List<T>> GetAll();
    }
}
