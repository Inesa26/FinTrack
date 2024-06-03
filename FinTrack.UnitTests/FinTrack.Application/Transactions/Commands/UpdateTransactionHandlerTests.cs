using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Application.Transactions.Commands;
using FinTrack.Domain.Enum;
using FinTrack.Domain.Model;
using Microsoft.Extensions.Logging;
using Moq;

namespace FinTrack.UnitTests.FinTrack.Application.Transactions.Commands
{
    public class UpdateTransactionHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<UpdateTransactionHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateTransactionHandler _handler;

        public UpdateTransactionHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<UpdateTransactionHandler>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateTransactionHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsUpdatedTransactionDto()
        {
            // Arrange
            DateTime date = DateTime.UtcNow;
            var request = new UpdateTransactionCommand(1, 255.00m, date, "Test", 2);
            var existingTransaction = new Transaction(1, 300.00m, date, "Test", 1);
            var updatedTransaction = new Transaction(1, 255.00m, date, "Test", 2);
            var existingCategory = new Category("Social", TransactionType.Expense, 1);
            var updatedTransactionDto = new TransactionDto
            {
                AccountId = updatedTransaction.AccountId,
                Amount = updatedTransaction.Amount,
                Date = updatedTransaction.Date,
                Description = updatedTransaction.Description,
                CategoryId = updatedTransaction.CategoryId
            };


            _unitOfWorkMock.Setup(uow => uow.TransactionRepository.Get(request.TransactionId)).ReturnsAsync(existingTransaction);
            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.Get(request.CategoryId)).ReturnsAsync(existingCategory);
            _unitOfWorkMock.Setup(uow => uow.TransactionRepository.Update(request.TransactionId, It.IsAny<Transaction>())).ReturnsAsync(updatedTransaction);
            _mapperMock.Setup(mapper => mapper.Map<TransactionDto>(It.IsAny<Transaction>())).Returns(updatedTransactionDto);

            // Act
            var result = await _handler.Handle(request, default);

            // Assert
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(), Times.Once);


            Assert.Equal(existingTransaction.Id, result.Id);
            Assert.Equal(request.Date, result.Date);
            Assert.Equal(request.Description, result.Description);
            Assert.Equal(request.Amount, result.Amount);
            Assert.Equal(request.CategoryId, result.CategoryId);
        }

        [Fact]
        public async Task Handle_TransactionNotFound_ThrowsException()
        {
            // Arrange
            var request = new UpdateTransactionCommand(1, 255.00m, DateTime.UtcNow, "Test", 2);

            _unitOfWorkMock.Setup(uow => uow.TransactionRepository.Get(request.TransactionId)).ReturnsAsync((Transaction?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, default));
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_CategoryNotFound_ThrowsException()
        {
            // Arrange
            var request = new UpdateTransactionCommand(1, 255.00m, DateTime.UtcNow, "Test", 2);
            var existingTransaction = new Transaction(1, 300.00m, DateTime.UtcNow, "Test", 1);

            _unitOfWorkMock.Setup(uow => uow.TransactionRepository.Get(request.TransactionId)).ReturnsAsync(existingTransaction);
            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.Get(request.CategoryId)).ReturnsAsync((Category?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, default));
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once);
        }
    }
}
