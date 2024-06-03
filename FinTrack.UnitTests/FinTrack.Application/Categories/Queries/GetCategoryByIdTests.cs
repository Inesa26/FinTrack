using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Categories.Queries;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using FinTrack.Domain.Model;
using Microsoft.Extensions.Logging;
using Moq;

namespace FinTrack.UnitTests.FinTrack.Application.Categories.Queries
{
    public class GetCategoryByIdHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<GetCategoryByIdHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetCategoryByIdHandler _handler;

        public GetCategoryByIdHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<GetCategoryByIdHandler>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetCategoryByIdHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ExistingCategory_ReturnsCategoryDto()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category("Test Category", TransactionType.Expense, categoryId);
            var categoryDto = new CategoryDto { Id = 1, Title = category.Title, IconId = category.IconId };

            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.Get(It.IsAny<int>())).ReturnsAsync(category);
            _mapperMock.Setup(mapper => mapper.Map<CategoryDto>(category)).Returns(categoryDto);

            // Act
            var result = await _handler.Handle(new GetCategoryByIdQuery(categoryId), default);

            // Assert
            Assert.Equal(categoryDto.Id, result.Id);
            Assert.Equal(categoryDto.Title, result.Title);
            Assert.Equal(categoryDto.IconId, result.IconId);
        }

        [Fact]
        public async Task Handle_NonExistingCategory_ThrowsException()
        {
            // Arrange
            var categoryId = 1;

            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.Get(It.IsAny<int>())).ReturnsAsync((Category?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(new GetCategoryByIdQuery(categoryId), default));
        }
    }
}
