using FinTrack.Application.Categories.Commands;
using FinTrack.Application.Categories.Queries;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using FinTrack.IntegrationTests.Helpers;
using FinTrack.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FinTrack.IntegrationTests.Controllers
{
    public class CategoriesControllerIntegrationTests
    {
        private CategoriesController CreateCategoriesController(DataContextBuilder contextBuilder)
        {
            var dbContext = contextBuilder.GetContext();
            var logger = TestHelpers.CreateLogger<GetAllCategoriesHandler>();
            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(dbContext, logger, mapper);

            return new CategoriesController(mediator);
        }

     /*   [Fact]
        public async Task GetAllCategories_ReturnsAllCategories()
        {
            // Arrange
            var categoryCount = 3;
            var iconCount = 3;

            using var contextBuilder = new DataContextBuilder();
            contextBuilder.SeedIcons(iconCount);
            contextBuilder.SeedCategories(categoryCount);

            var controller = CreateCategoriesController(contextBuilder);

            // Act
            var actionResult = await controller.GetAllCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var categories = okResult.Value as List<CategoryDto>;

            Assert.NotNull(categories);
            Assert.Equal(categoryCount, categories.Count);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }
     */
        [Fact]
        public async Task GetCategoryById_ReturnsCategory()
        {
            // Arrange
            var categoryCount = 3;
            var iconCount = 3;

            using var contextBuilder = new DataContextBuilder();
            contextBuilder.SeedIcons(iconCount);
            contextBuilder.SeedCategories(categoryCount);

            var controller = CreateCategoriesController(contextBuilder);
            var categoryId = 1;

            // Act
            var actionResult = await controller.GetCategoryById(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var category = okResult.Value as CategoryDto;

            Assert.NotNull(category);
            Assert.Equal(categoryId, category.Id);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task CreateCategory_ReturnsCreatedCategory()
        {
            // Arrange
            var title = "Test Category";
            var iconId = 1;
            var type = TransactionType.Expense;

            using var contextBuilder = new DataContextBuilder();
            contextBuilder.SeedIcons(3);

            var controller = CreateCategoriesController(contextBuilder);
            var createCategoryCommand = new CreateCategoryCommand(title, iconId, type);

            // Act
            var actionResult = await controller.CreateCategory(createCategoryCommand);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult);
            var category = createdResult.Value as CategoryDto;

            Assert.NotNull(category);
            Assert.Equal(createCategoryCommand.Title, category.Title);
            Assert.Equal(createCategoryCommand.IconId, category.IconId);
            Assert.Equal((int)HttpStatusCode.Created, createdResult.StatusCode);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsOk()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            contextBuilder.SeedIcons(3);
            contextBuilder.SeedCategories(1);

            var controller = CreateCategoriesController(contextBuilder);
            var categoryId = 1;
            var updateCategoryCommand = new UpdateCategoryCommand(categoryId, "Updated Category", 1, TransactionType.Expense);

            // Act
            var actionResult = await controller.UpdateCategory(updateCategoryCommand);

            // Assert
            var okResult = Assert.IsType<OkResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsNoContent()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            contextBuilder.SeedIcons(3);
            contextBuilder.SeedCategories(1);

            var controller = CreateCategoriesController(contextBuilder);
            var categoryId = 1;

            // Act
            var actionResult = await controller.DeleteCategory(categoryId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
        }
    }
}
