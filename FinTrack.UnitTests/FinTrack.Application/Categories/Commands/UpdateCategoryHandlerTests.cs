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
    public class UpdateCategoryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<UpdateCategoryHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateCategoryHandler _handler;

        public UpdateCategoryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<UpdateCategoryHandler>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateCategoryHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsUpdatedCategoryDto()
        {
            // Arrange
            var request = new UpdateCategoryCommand(1, "Updated Category", 1, TransactionType.Expense);
            var existingCategory = new Category("Test Category", TransactionType.Expense, 1);
            var expectedUpdatedCategory = new Category("Updated Category", TransactionType.Expense, 1);
            var expectedCategoryDto = new CategoryDto
            {
                Title = expectedUpdatedCategory.Title,
                IconId = expectedUpdatedCategory.IconId
            };

            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.Get(request.CategoryId)).ReturnsAsync(existingCategory);
            _unitOfWorkMock.Setup(uow => uow.IconRepository.Get(request.IconId)).ReturnsAsync(new Icon(new byte[0], TransactionType.Expense, "title"));
            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.Update(request.CategoryId, It.IsAny<Category>())).ReturnsAsync(expectedUpdatedCategory);
            _mapperMock.Setup(mapper => mapper.Map<CategoryDto>(expectedUpdatedCategory)).Returns(expectedCategoryDto);

            // Act
            var result = await _handler.Handle(request, default);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitTransactionAsync(), Times.Once);

            Assert.Equal(expectedCategoryDto.Title, result.Title);
            Assert.Equal(expectedCategoryDto.IconId, result.IconId);
        }

        [Fact]
        public async Task Handle_CategoryNotFound_ThrowsException()
        {
            // Arrange
            var request = new UpdateCategoryCommand(1, "Updated Category", 1, TransactionType.Expense);

            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.Get(request.CategoryId)).ReturnsAsync((Category?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, default));
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_IconNotFound_ThrowsException()
        {
            // Arrange
            var request = new UpdateCategoryCommand(1, "Updated Category", 1, TransactionType.Expense);
            var existingCategory = new Category("Test Category", TransactionType.Expense, 1);

            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.Get(request.CategoryId)).ReturnsAsync(existingCategory);
            _unitOfWorkMock.Setup(uow => uow.IconRepository.Get(request.IconId)).ReturnsAsync((Icon?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, default));
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once);
        }

    }
}
