using FinTrack.Domain.Model;

namespace FinTrack.Application.Abstractions
{
    public interface IUnitOfWork
    {
        public IRepository<Category> CategoryRepository { get; }
        public IRepository<Icon> IconRepository { get; }
        Task SaveAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
