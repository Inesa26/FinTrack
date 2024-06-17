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
    public class CreateCategoryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<CreateCategoryHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IRepository<Category>> _categoryRepository;
        private readonly CreateCategoryHandler _handler;

        public CreateCategoryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<CreateCategoryHandler>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateCategoryHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
            _categoryRepository = new Mock<IRepository<Category>>();
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsCategoryDto()
        {
            // Arrange
            var request = new CreateCategoryCommand("Test Category", 1, TransactionType.Expense);
            var categoryList = new List<Category>();
            byte[] iconData = new byte[] { 0xFF, 0xAA, 0x55, 0x00 };
            TransactionType transactionType = TransactionType.Expense;
            var existingIcon = new Icon(iconData, transactionType, "test");
            var category = new Category("Test Category", TransactionType.Expense, existingIcon.Id);
            var expectedCategoryDto = new CategoryDto
            {
                Title = category.Title,
                IconId = category.IconId
            };

            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.GetAll()).ReturnsAsync(categoryList);
            _unitOfWorkMock.Setup(uow => uow.IconRepository.Get(It.IsAny<int>())).ReturnsAsync(existingIcon);
            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.Add(It.IsAny<Category>())).ReturnsAsync(category);
            _mapperMock.Setup(mapper => mapper.Map<CategoryDto>(It.IsAny<Category>())).Returns(expectedCategoryDto);

            // Act
            var result = await _handler.Handle(request, default);

            // Assert
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(), Times.Once);

            Assert.Equal(expectedCategoryDto.Title, result.Title);
            Assert.Equal(expectedCategoryDto.IconId, result.IconId);
        }

        [Fact]
        public async Task Handle_CategoryAlreadyExists_ThrowsException()
        {
            // Arrange
            var request = new CreateCategoryCommand("Test Category", 1, TransactionType.Expense);
            var categoryList = new List<Category>
             {
                 new Category("Test Category", TransactionType.Expense, 1)
              };

            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.GetAll()).ReturnsAsync(categoryList);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, default));
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_IconNotFound_ThrowsException()
        {
            // Arrange
            var request = new CreateCategoryCommand("Test Category", 1, TransactionType.Expense);
            var categoryList = new List<Category>();

            _unitOfWorkMock.Setup(uow => uow.CategoryRepository.GetAll()).ReturnsAsync(categoryList);
            _unitOfWorkMock.Setup(uow => uow.IconRepository.Get(It.IsAny<int>())).ReturnsAsync((Icon?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, default));
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once);
        }

    }
}
