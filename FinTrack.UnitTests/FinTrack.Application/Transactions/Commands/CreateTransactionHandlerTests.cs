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
    public class CreateTransactionHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<CreateTransactionHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateTransactionHandler _handler;

        public CreateTransactionHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<CreateTransactionHandler>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateTransactionHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsTransactionDto()
        {
            // Arrange
            var request = new CreateTransactionCommand(1, 255.00m, DateTime.UtcNow, "Test", 2, TransactionType.Expense);
            var existingCategory = new Category("Social", TransactionType.Expense, 2);
            var account = new Account("userId");
            var transaction = new Transaction(1, 255.00m, DateTime.UtcNow, "Test", 2, TransactionType.Expense);
            var expectedTransactionDto = new TransactionDto
            {
                Id = transaction.Id,
                AccountId = transaction.AccountId,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Description = transaction.Description,
                CategoryId = transaction.CategoryId,
                TransactionType = transaction.Type
            };

            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.Get(request.CategoryId)).ReturnsAsync(existingCategory);
            _unitOfWorkMock.Setup(uow => uow.AccountRepository.Get(request.AccountId)).ReturnsAsync(account);
            _unitOfWorkMock.Setup(uow => uow.TransactionRepository.Add(It.IsAny<Transaction>())).ReturnsAsync(transaction);
            _mapperMock.Setup(mapper => mapper.Map<TransactionDto>(It.IsAny<Transaction>())).Returns(expectedTransactionDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(), Times.Once);

            Assert.Equal(expectedTransactionDto.Id, result.Id);
            Assert.Equal(expectedTransactionDto.AccountId, result.AccountId);
            Assert.Equal(expectedTransactionDto.Amount, result.Amount);
            Assert.Equal(expectedTransactionDto.Date, result.Date);
            Assert.Equal(expectedTransactionDto.Description, result.Description);
            Assert.Equal(expectedTransactionDto.CategoryId, result.CategoryId);
        }

        [Fact]
        public async Task Handle_CategoryNotFound_ThrowsException()
        {
            // Arrange
            var request = new CreateTransactionCommand(1,255.00m, DateTime.UtcNow, "Test", 2, TransactionType.Expense);

            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.Get(request.CategoryId)).ReturnsAsync((Category?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, default));
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once);
        }
    }
}
