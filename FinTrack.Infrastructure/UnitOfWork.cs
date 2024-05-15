using FinTrack.Application.Abstractions;
using FinTrack.Domain.Model;
using FinTrack.Infrastructure.Data;

namespace FinTrack.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FinTrackDbContext _context;

        public UnitOfWork(FinTrackDbContext context, IRepository<Category> categoryRepository,
            IRepository<Icon> iconRepository, IRepository<Transaction> transactionRepository,
            IUserRepository userRepository)
        {
            _context = context;
            CategoryRepository = categoryRepository;
            IconRepository = iconRepository;
            TransactionRepository = transactionRepository;
            UserRepository = userRepository;
        }

        public IRepository<Category> CategoryRepository { get; private set; }
        public IRepository<Icon> IconRepository { get; private set; }
        public IRepository<Transaction> TransactionRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }

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
