using FinTrack.Application.Common.Models;
using FinTrack.Application.Responses;
using FinTrack.Application.Transactions.Commands;
using FinTrack.Application.Transactions.Queries;
using FinTrack.IntegrationTests.Helpers;
using FinTrack.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FinTrack.IntegrationTests.Controllers
{
    public class TransactionsControllerIntegrationTests
    {
        private TransactionsController CreateTransactionsController(DataContextBuilder contextBuilder)
        {
            var dbContext = contextBuilder.GetContext();
            var logger = TestHelpers.CreateLogger<GetAllTransactionsHandler>();
            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(dbContext, logger, mapper);
            var unitOfWork = TestHelpers.CreateUnitOfWork(dbContext);



            return new TransactionsController(mediator, unitOfWork);
        }

        [Fact]
        public async Task GetTransactionById_ReturnsTransaction()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            await contextBuilder.SeedUsersAsync(1);
            await contextBuilder.SeedAccountsAsync(1);
            contextBuilder.SeedIcons(1);
            contextBuilder.SeedCategories(1);
            await contextBuilder.SeedTransactionsAsync(1);

            var controller = CreateTransactionsController(contextBuilder);
            var transactionId = 1;

            // Act
            var actionResult = await controller.GetTransactionById(transactionId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var transaction = Assert.IsType<TransactionDto>(okObjectResult.Value);

            Assert.NotNull(transaction);
            Assert.Equal(transactionId, transaction.Id);
            Assert.Equal((int)HttpStatusCode.OK, (actionResult.Result as OkObjectResult)?.StatusCode);
        }

       /* [Fact]
        public async Task GetTransactionsByAccountId_ReturnsTransactions()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            await contextBuilder.SeedUsersAsync(1);
            await contextBuilder.SeedAccountsAsync(1);
            contextBuilder.SeedIcons(1);
            contextBuilder.SeedCategories(1);
            await contextBuilder.SeedTransactionsAsync(10);

            var controller = CreateTransactionsController(contextBuilder);
            var accountId = 1;
            var pageIndex = 1;
            var pageSize = 10;
            var sortBy = "Date";
            var sortOrder = "desc";

            // Act
            var actionResult = await controller.GetTransactionsByAccountId(accountId, pageIndex, pageSize, sortBy, sortOrder);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var paginatedResult = Assert.IsType<PaginatedResult<TransactionDto>>(okObjectResult.Value);
            var transactions = paginatedResult.Items;

            Assert.NotNull(transactions);
            Assert.Equal(10, transactions.Count);
            Assert.Equal((int)HttpStatusCode.OK, okObjectResult.StatusCode);
        }*/


        /*
        [Fact]
        public async Task CreateTransaction_ReturnsCreatedTransaction()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            await contextBuilder.SeedUsersAsync(1);
            await contextBuilder.SeedAccountsAsync(1);
            contextBuilder.SeedIcons(1);
            contextBuilder.SeedCategories(1);

            var controller = CreateTransactionsController(contextBuilder);
           

            // Mocking the unitOfWork.AccountRepository.GetSingle method
            var mockRepository = new Mock<IRepository<Account>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.AccountRepository).Returns(mockRepository.Object);

            var userId = "testuser1"; // Replace with your test user's ID
            var account = new Account ("testuser1"); // Mocked account data
            mockRepository.Setup(repo => repo.GetSingle(It.IsAny<Func<IQueryable<Account>, IQueryable<Account>>>()))
                          .ReturnsAsync(account);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId)
                    }))
                }
            };

            var transactionRequest = new TransactionRequest(100, DateTime.Now, "Transaction from test", 1, TransactionType.Expense);
            
            // Act
            var actionResult = await controller.CreateTransaction(transactionRequest);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult);
            var transactionDto = Assert.IsType<TransactionDto>(createdResult.Value);

            Assert.NotNull(transactionDto);
            Assert.Equal(account.Id, transactionDto.AccountId);
            Assert.Equal(transactionRequest.Amount, transactionDto.Amount);
            Assert.Equal((int)HttpStatusCode.Created, createdResult.StatusCode);
        }
    

        */


      /*  [Fact]
        public async Task UpdateTransaction_ReturnsOk()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            await contextBuilder.SeedUsersAsync(1);
            await contextBuilder.SeedAccountsAsync(1);
            contextBuilder.SeedIcons(1);
            contextBuilder.SeedCategories(1);
            await contextBuilder.SeedTransactionsAsync(1);

            var controller = CreateTransactionsController(contextBuilder);
            var transactionId = 1;
            decimal amount = 150;
            DateTime date = DateTime.Now;
            string description = "Updated Transaction from test";
            int categoryId = 1;

            var updateTransactionCommand = new UpdateTransactionCommand(transactionId, amount, date, description, categoryId);

            // Act
            var actionResult = await controller.UpdateTransaction(updateTransactionCommand);

            // Assert
            var okResult = Assert.IsType<OkResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }*/

        [Fact]
        public async Task DeleteTransaction_ReturnsNoContent()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            await contextBuilder.SeedUsersAsync(1);
            await contextBuilder.SeedAccountsAsync(1);
            contextBuilder.SeedIcons(1);
            contextBuilder.SeedCategories(1);
            await contextBuilder.SeedTransactionsAsync(1);

            var controller = CreateTransactionsController(contextBuilder);
            var transactionId = 1;

            // Act
            var actionResult = await controller.DeleteTransaction(transactionId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
        }
    }
}
