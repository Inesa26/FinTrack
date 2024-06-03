﻿using FinTrack.Application.Icons.Commands;
using FinTrack.Application.Icons.Queries;
using FinTrack.Application.Responses;
using FinTrack.IntegrationTests.Helpers;
using FinTrack.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace FinTrack.IntegrationTests.Controllers
{
    public class IconsControllerIntegrationTests
    {
        private IconsController CreateIconsController(DataContextBuilder contextBuilder)
        {
            var dbContext = contextBuilder.GetContext();
            var logger = TestHelpers.CreateLogger<GetAllIconsHandler>();
            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(dbContext, logger, mapper);

            return new IconsController(mediator);
        }

        [Fact]
        public async Task GetAllIcons_ReturnsAllIcons()
        {
            using var contextBuilder = new DataContextBuilder();
            // Arrange
            var iconCount = 3;
            contextBuilder.SeedIcons(iconCount);
            var controller = CreateIconsController(contextBuilder);

            // Act
            var actionResult = await controller.GetAllIcons();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var icons = okResult.Value as List<IconDto>;

            Assert.NotNull(icons);
            Assert.Equal(iconCount, icons.Count);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetIconById_ReturnsIcon()
        {
            using var contextBuilder = new DataContextBuilder();
            // Arrange
            var iconCount = 3;
            contextBuilder.SeedIcons(iconCount);
            var controller = CreateIconsController(contextBuilder);
            var iconId = 1;

            // Act
            var actionResult = await controller.GetIconById(iconId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var icon = okResult.Value as IconDto;

            Assert.NotNull(icon);
            Assert.Equal(iconId, icon.Id);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task CreateIcon_ReturnsCreatedIcon()
        {
            using var contextBuilder = new DataContextBuilder();
            // Arrange
            var iconData = "D:\\Amdaris_Internship\\FinTrack\\Icons\\icon1.png";
            var controller = CreateIconsController(contextBuilder);
            var createIconCommand = new CreateIconCommand(iconData);

            // Act
            var actionResult = await controller.CreateIcon(createIconCommand);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult);
            var icon = createdResult.Value as IconDto;

            Assert.NotNull(icon);
            Assert.Equal((int)HttpStatusCode.Created, createdResult.StatusCode);
        }

        [Fact]
        public async Task UpdateIcon_ReturnsOk()
        {
            using var contextBuilder = new DataContextBuilder();
            // Arrange
            var updatedIconData = "D:\\Amdaris_Internship\\FinTrack\\Icons\\icon1.png";
            contextBuilder.SeedIcons(1);
            var controller = CreateIconsController(contextBuilder);
            var updateIconCommand = new UpdateIconCommand(1, updatedIconData);

            // Act
            var actionResult = await controller.UpdateIcon(updateIconCommand);

            // Assert
            var okResult = Assert.IsType<OkResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteIcon_ReturnsNoContent()
        {
            using var contextBuilder = new DataContextBuilder();
            // Arrange
            contextBuilder.SeedIcons(1);
            var controller = CreateIconsController(contextBuilder);
            var iconId = 1;

            // Act
            var actionResult = await controller.DeleteIcon(iconId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(actionResult);
            Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
        }
    }
}
