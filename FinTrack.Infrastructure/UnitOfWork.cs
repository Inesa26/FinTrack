using FinTrack.Application.Abstractions;
using FinTrack.Domain.Model;
using FinTrack.Infrastructure.Data;

namespace FinTrack.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FinTrackDbContext _context;

        public UnitOfWork(FinTrackDbContext context, IRepository<Category> categoryRepository,
            IRepository<Icon> iconRepository)
        {
            _context = context;
            CategoryRepository = categoryRepository;
            IconRepository = iconRepository;
        }

        public IRepository<Category> CategoryRepository { get; private set; }
        public IRepository<Icon> IconRepository { get; private set; }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
