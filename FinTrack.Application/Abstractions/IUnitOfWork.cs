using FinTrack.Domain.Model;

namespace FinTrack.Application.Abstractions
{
    public interface IUnitOfWork
    {
        IRepository<Category> CategoryRepository { get; }
        IRepository<Icon> IconRepository { get; }
        IRepository<Transaction> TransactionRepository { get; }
        IUserRepository UserRepository { get; }
        Task SaveAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
