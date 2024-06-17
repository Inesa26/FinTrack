/*using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Categories.Queries;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using FinTrack.Domain.Model;
using Microsoft.Extensions.Logging;
using Moq;

namespace FinTrack.UnitTests.FinTrack.Application.Categories.Queries
{
    public class GetAllCategoriesHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<GetAllCategoriesHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllCategoriesHandler _handler;

        public GetAllCategoriesHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<GetAllCategoriesHandler>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllCategoriesHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsListOfCategoryDtos()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category("Category 1", TransactionType.Income, 1),
                new Category("Category 2", TransactionType.Expense, 2),
                new Category("Category 3", TransactionType.Income, 3)
            };

            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.GetAll()).ReturnsAsync(categories);
            _mapperMock.Setup(mapper => mapper.Map<CategoryDto>(It.IsAny<Category>()))
                       .Returns<Category>(each => new CategoryDto { Id = each.Id, Title = each.Title, IconId = each.IconId });

            // Act
            var result = await _handler.Handle(new GetAllCategoriesQuery(), default);

            // Assert
            Assert.Equal(categories.Count, result.Count);
        }

        [Fact]
        public async Task Handle_ThrowsException()
        {
            // Arrange
            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.GetAll()).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(new GetAllCategoriesQuery(), default));
        }
    }
}
*/