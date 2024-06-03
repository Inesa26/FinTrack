using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Application.Transactions.Commands;
using FinTrack.Domain.Model;
using Microsoft.Extensions.Logging;
using Moq;


namespace FinTrack.UnitTests.FinTrack.Application.Transactions.Commands
{
    public class DeleteTransactionHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<DeleteTransactionHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DeleteTransactionHandler _handler;

        public DeleteTransactionHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<DeleteTransactionHandler>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new DeleteTransactionHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsDeletedTransactionDto()
        {
            // Arrange
            var transactionId = 1;
            var request = new DeleteTransactionCommand(transactionId);
            var transaction = new Transaction(1, 255.00m, DateTime.UtcNow, "Test", 2);
            var deletedTransactionDto = new TransactionDto
            {
                AccountId = transaction.AccountId,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Description = transaction.Description,
                CategoryId = transaction.CategoryId
            };

            _unitOfWorkMock.Setup(uow => uow.TransactionRepository.Get(It.IsAny<int>())).ReturnsAsync(transaction);
            _unitOfWorkMock.Setup(uow => uow.TransactionRepository.Delete(It.IsAny<Transaction>())).ReturnsAsync(transaction);
            _mapperMock.Setup(mapper => mapper.Map<TransactionDto>(It.IsAny<Transaction>())).Returns(deletedTransactionDto);

            // Act
            var result = await _handler.Handle(request, default);

            // Assert
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(), Times.Once);

            Assert.Equal(deletedTransactionDto.AccountId, result.AccountId);
            Assert.Equal(deletedTransactionDto.Amount, result.Amount);
            Assert.Equal(deletedTransactionDto.Date, result.Date);
            Assert.Equal(deletedTransactionDto.Description, result.Description);
            Assert.Equal(deletedTransactionDto.CategoryId, result.CategoryId);
        }

        [Fact]
        public async Task Handle_TransactionNotFound_ThrowsException()
        {
            // Arrange
            var transactionId = 1;
            var request = new DeleteTransactionCommand(transactionId);

            _unitOfWorkMock.Setup(uow => uow.TransactionRepository.Get(transactionId)).ReturnsAsync((Transaction?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, default));
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once);
        }
    }
}
