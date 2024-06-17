using FinTrack.Application.Common.Models;
using FinTrack.Domain.Model;
using System.Linq.Expressions;

namespace FinTrack.Application.Abstractions
{
    public interface IRepository<T>
    {
        Task<T> Add(T entity);
        Task<T> Delete(T enitity);
        Task<T> Update(int id, T entity);
        Task<T?> Get(int id);
        Task<List<T>> GetAll();
        Task<List<T>> Filter(Func<IQueryable<T>, IQueryable<T>> filterFunc);
        Task<T?> GetSingle(Func<IQueryable<T>, IQueryable<T>> filterFunc);

        Task<PaginatedResult<T>> GetPaginated(int pageIndex, 
            int pageSize, 
            Func<IQueryable<T>, 
            IQueryable<T>> filterFunc = null);
    }
}
