using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Application.Transactions.Queries;
using FinTrack.Domain.Model;
using Microsoft.Extensions.Logging;
using Moq;

namespace FinTrack.UnitTests.FinTrack.Application.Categories.Queries
{
    public class GetTransactionByIdHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<GetTransactionByIdHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetTransactionByIdHandler _handler;

        public GetTransactionByIdHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<GetTransactionByIdHandler>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetTransactionByIdHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ExistingTransaction_ReturnsTransactionDto()
        {
            // Arrange
            int transactionId = 1;
            var transaction = new Transaction(1, 100.0m, DateTime.Now, "Transaction 1", 1);
            var transactionDto = new TransactionDto
            {
                Id = transaction.Id,
                AccountId = transaction.AccountId,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Description = transaction.Description,
                CategoryId = transaction.CategoryId
            };

            _unitOfWorkMock.Setup(uow => uow.TransactionRepository.Get(It.IsAny<int>())).ReturnsAsync(transaction);
            _mapperMock.Setup(mapper => mapper.Map<TransactionDto>(transaction)).Returns(transactionDto);

            // Act
            var result = await _handler.Handle(new GetTransactionByIdQuery(transactionId), default);

            // Assert
            Assert.Equal(transactionDto.Id, result.Id);
            Assert.Equal(transactionDto.AccountId, result.AccountId);
            Assert.Equal(transactionDto.Amount, result.Amount);
            Assert.Equal(transactionDto.Date, result.Date);
            Assert.Equal(transactionDto.Description, result.Description);
            Assert.Equal(transactionDto.CategoryId, result.CategoryId);
        }

        [Fact]
        public async Task Handle_NonExistingCategory_ThrowsException()
        {
            // Arrange
            var transactionId = 1;

            _unitOfWorkMock.Setup(uow => uow.TransactionRepository.Get(It.IsAny<int>())).ReturnsAsync((Transaction?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(new GetTransactionByIdQuery(transactionId), default));
        }
    }
}
