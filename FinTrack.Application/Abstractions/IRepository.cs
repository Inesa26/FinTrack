using FinTrack.Domain.Model;

namespace FinTrack.Application.Abstractions
{
    public interface IRepository<T> where T : Entity
    {
        Task<T> Add(T entity);
        Task<T> Delete(T enitity);
        Task<T> Update(int id, T entity);
        Task<T?> Get(int id);
        Task<List<T>> GetAll();
    }
}
