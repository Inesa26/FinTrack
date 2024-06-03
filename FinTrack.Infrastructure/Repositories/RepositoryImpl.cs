using FinTrack.Application.Abstractions;
using FinTrack.Application.Common.Models;
using FinTrack.Domain.Model;
using FinTrack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Repositories
{
    public class RepositoryImpl<T> : IRepository<T> where T : Entity
    {
        private readonly FinTrackDbContext _context;

        public RepositoryImpl(FinTrackDbContext context)
        {
            _context = context;
        }

        public async Task<T> Add(T entity)
        {
            _context.Add(entity);
            return entity;
        }

        public async Task<T> Delete(T entity)
        {
            _context.Remove(entity);
            return entity;
        }
        public async Task<T> Update(int id, T entity)
        {
            _context.Update(entity);
            return entity;
        }
        public async Task<T?> Get(int id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> Filter(Func<IQueryable<T>, IQueryable<T>> filterFunc)
        {
            var query = _context.Set<T>().AsQueryable();
            query = filterFunc(query);
            return await query.ToListAsync();
        }
        public async Task<PaginatedResult<T>> GetPaginated(int pageIndex, int pageSize, Func<IQueryable<T>, IQueryable<T>> filterFunc = null)
        {
            IQueryable<T> query = _context.Set<T>();


            if (filterFunc != null)
            {
                query = filterFunc(query);
            }

            int totalCount = await query.CountAsync();
            List<T> items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<T>(items, totalCount, pageIndex, pageSize);
        }

    }
}
