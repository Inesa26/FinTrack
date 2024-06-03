using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Categories.Commands;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using FinTrack.Domain.Model;
using Microsoft.Extensions.Logging;
using Moq;

namespace FinTrack.UnitTests.FinTrack.Application.Categories.Commands
{
    public class DeleteCategoryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<DeleteCategoryHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DeleteCategoryHandler _handler;

        public DeleteCategoryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<DeleteCategoryHandler>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new DeleteCategoryHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_DeletesCategory()
        {
            // Arrange
            var categoryId = 1;
            var request = new DeleteCategoryCommand(categoryId);
            var category = new Category("Test Category", TransactionType.Expense, 1);
            var expectedCategoryDto = new CategoryDto
            {
                Title = category.Title,
                IconId = category.IconId
            };

            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.Get(It.IsAny<int>())).ReturnsAsync(category);
            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.Delete(It.IsAny<Category>())).ReturnsAsync(category);
            _mapperMock.Setup(mapper => mapper.Map<CategoryDto>(It.IsAny<Category>())).Returns(expectedCategoryDto);

            // Act
            var result = await _handler.Handle(request, default);

            // Assert
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitTransactionAsync(), Times.Once);

            Assert.Equal(expectedCategoryDto.Title, result.Title);
            Assert.Equal(expectedCategoryDto.IconId, result.IconId);
        }

        [Fact]
        public async Task Handle_NonExistingCategory_ThrowsException()
        {
            // Arrange
            var categoryId = 1;
            var request = new DeleteCategoryCommand(categoryId);

            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.Get(It.IsAny<int>())).ReturnsAsync((Category?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, default));
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once);
           
        }
    }
}
