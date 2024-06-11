using FinTrack.Application.Accounts.Commands;
using FinTrack.Application.Auth.Commands;
using FinTrack.Application.Users.Commands;
using FinTrack.Application.Responses;
using FinTrack.IntegrationTests.Helpers;
using FinTrack.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.IntegrationTests.Controllers
{
    public class AccountsControllerIntegrationTests
    {
        private AccountsController CreateAccountsController(DataContextBuilder contextBuilder)
        {
            var dbContext = contextBuilder.GetContext();
            var logger = TestHelpers.CreateLogger<AccountsController>();
            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(dbContext, logger, mapper);

            return new AccountsController(mediator);
        }

        [Fact]
        public async Task Register_CreatesUserAndAccount_ReturnsOk()
        {
            using var contextBuilder = new DataContextBuilder();
            string email = "test@gmail.com";
            string password = "Password123@";
            string firstName = "Test";
            string lastName = "Test";
            // Arrange
            var createUserCommand = new CreateUserCommand(firstName, lastName, email, password);

            var controller = CreateAccountsController(contextBuilder);

            // Act
            var actionResult = await controller.Register(createUserCommand);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var result = okResult.Value as dynamic;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task Register_InvalidModel_ReturnsBadRequest()
        {
            using var contextBuilder = new DataContextBuilder();
            string email = "notemail";
            string password = "password";
            string firstName = "";
            string lastName = "";
            // Arrange
            var createUserCommand = new CreateUserCommand(firstName, lastName, email, password);

            var controller = CreateAccountsController(contextBuilder);
            controller.ModelState.AddModelError("Email", "Invalid email format");

            // Act
            var actionResult = await controller.Register(createUserCommand);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOk()
        {
            using var contextBuilder = new DataContextBuilder();
            // Arrange
            await contextBuilder.SeedUsersAsync(1);
                  var dbContext = contextBuilder.GetContext();
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == "user0@gmail.com");
            Assert.NotNull(user);
            var signInCommand = new LogInCommand
            {
                Email = "user0@gmail.com",
                Password = "Password123@"
            };

            var controller = CreateAccountsController(contextBuilder);

            // Act
            var actionResult = await controller.Login(signInCommand);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var jwtToken = okResult.Value;

            Assert.NotNull(jwtToken);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            using var contextBuilder = new DataContextBuilder();
            // Arrange
            await contextBuilder.SeedUsersAsync(1);
            var signInCommand = new LogInCommand
            {
                Email = "user0@gmail.com",
                Password = "WrongPassword"
            };

            var controller = CreateAccountsController(contextBuilder);

            // Act
            var actionResult = await controller.Login(signInCommand);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.Unauthorized, unauthorizedResult.StatusCode);
        }
    }
}
