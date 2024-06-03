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

            return new TransactionsController(mediator);
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

        [Fact]
        public async Task GetTransactionsByAccountId_ReturnsTransactions()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            await contextBuilder.SeedUsersAsync(1);
            await contextBuilder.SeedAccountsAsync(1);
            contextBuilder.SeedIcons(1);
            contextBuilder.SeedCategories(1);
            await contextBuilder.SeedTransactionsAsync(1);

            var controller = CreateTransactionsController(contextBuilder);
            var accountId = 1;

            // Act
            var actionResult = await controller.GetTransactionsByAccountId(accountId);

            // Log ActionResult for debugging
            Console.WriteLine("Action Result: " + actionResult);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var transactions = Assert.IsType<List<TransactionDto>>(okObjectResult.Value);

            Assert.NotNull(transactions);
            Assert.Equal(1, transactions.Count);
            Assert.Equal((int)HttpStatusCode.OK, okObjectResult.StatusCode);
        }


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
            int accountId = 1;
            decimal amount = 100;
            DateTime date = DateTime.Now;
            string description = "Transaction from test";
            int categoryId = 1;
            var createTransactionCommand = new CreateTransactionCommand(accountId, amount, date, description, categoryId);

            // Act
            var actionResult = await controller.CreateTransaction(createTransactionCommand);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult);
            var transaction = Assert.IsType<TransactionDto>(createdResult.Value);

            Assert.NotNull(transaction);
            Assert.Equal(createTransactionCommand.AccountId, transaction.AccountId);
            Assert.Equal(createTransactionCommand.Amount, transaction.Amount);
            Assert.Equal((int)HttpStatusCode.Created, createdResult.StatusCode);
        }

        [Fact]
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
        }

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
