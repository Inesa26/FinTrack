using FinTrack.Application.Abstractions;
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
    }
}
