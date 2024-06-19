using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Application.Transactions.Queries;
using FinTrack.Domain.Enum;
using FinTrack.Domain.Model;
using Microsoft.Extensions.Logging;
using Moq;


namespace FinTrack.UnitTests.FinTrack.Application.Transactions.Queries
{
    public class GetAllTransactionsHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<GetAllTransactionsHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllTransactionsHandler _handler;
        private readonly int accountId = 1;

        public GetAllTransactionsHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<GetAllTransactionsHandler>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllTransactionsHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

    /*    [Fact]
        public async Task Handle_ValidRequest_ReturnsPaginatedResultOfTransactionDtos()
        {
            // Arrange
            var request = new GetAllTransactionsQuery(accountId, pageIndex: 1, pageSize: 10, sortBy: "Date", sortOrder: "asc");
            var transactions = new List<Transaction>
    {
        new Transaction(1, 100.0m, DateTime.Now, "Transaction 1", accountId, TransactionType.Expense),
        new Transaction(2, 200.0m, DateTime.Now, "Transaction 2", accountId, TransactionType.Expense)
    };

            var expectedTransactionDtos = transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                AccountId = t.AccountId,
                Amount = t.Amount,
                Date = t.Date,
                Description = t.Description,
                CategoryId = t.CategoryId
            }).ToList();

            _unitOfWorkMock.Setup(uow => uow.TransactionRepository.Filter(It.IsAny<Func<IQueryable<Transaction>, IQueryable<Transaction>>>()))
                .ReturnsAsync(transactions);

            _mapperMock.Setup(mapper => mapper.Map<TransactionDto>(It.IsAny<Transaction>()))
                .Returns((Transaction t) => new TransactionDto
                {
                    Id = t.Id,
                    AccountId = t.AccountId,
                    Amount = t.Amount,
                    Date = t.Date,
                    Description = t.Description,
                    CategoryId = t.CategoryId
                });

            // Act
            var result = await _handler.Handle(request, default);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.TransactionRepository.Filter(It.IsAny<Func<IQueryable<Transaction>, IQueryable<Transaction>>>()), Times.Once);

            Assert.Equal(expectedTransactionDtos.Count, result.Items.Count);

            for (int i = 0; i < expectedTransactionDtos.Count; i++)
            {
                Assert.True(AreEqualTransactionDtos(expectedTransactionDtos[i], result.Items[i]), $"TransactionDto at index {i} does not match.");
            }
        }*/


        [Fact]
        public async Task Handle_EmptyListReturnedWhenNoTransactionsForAccount()
        {
            // Arrange
            var request = new GetAllTransactionsQuery(accountId, pageIndex: 1, pageSize: 10, sortBy: "Date", sortOrder: "asc");
            var emptyTransactions = new List<Transaction>();

            _unitOfWorkMock.Setup(uow => uow.TransactionRepository.Filter(It.IsAny<Func<IQueryable<Transaction>, IQueryable<Transaction>>>()))
                .ReturnsAsync(emptyTransactions);

            // Act
            var result = await _handler.Handle(request, default);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.TransactionRepository.Filter(It.IsAny<Func<IQueryable<Transaction>, IQueryable<Transaction>>>()), Times.Once);
            Assert.Empty(result.Items);
        }


        private bool AreEqualTransactionDtos(TransactionDto dto1, TransactionDto dto2)
        {
            return dto1.Id == dto2.Id
                && dto1.AccountId == dto2.AccountId
                && dto1.Amount == dto2.Amount
                && dto1.Date == dto2.Date
                && dto1.Description == dto2.Description
                && dto1.CategoryId == dto2.CategoryId;
        }
    }
}
