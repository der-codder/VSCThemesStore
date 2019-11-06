using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VSCThemesStore.WebApi.Controllers;
using VSCThemesStore.WebApi.Controllers.Resources;
using VSCThemesStore.WebApi.Domain.Models;
using VSCThemesStore.WebApi.Domain.Repositories;
using Xunit;

namespace VSCThemesStore.WebApi.Tests.Controllers
{
    public class ThemesControllerTests : BaseControllerTests
    {
        [Fact]
        public async Task Get_WithoutParams_ReturnsFullThemeData()
        {
            const string expectedId = "expectedId_test";
            const string expectedLabel = "themeLabel_test";
            var themes = new List<Theme> { new Theme { Label = "themeLabel_test" } };

            _mock.Provide(CreateMapper());
            _mock.Mock<IExtensionsMetadataRepository>()
                .Setup(x => x.GetExtensionMetadata(expectedId))
                .ReturnsAsync(new ExtensionMetadata { Id = expectedId });
            _mock.Mock<IThemeRepository>()
                .Setup(x => x.GetTheme(expectedId))
                .ReturnsAsync(new VSCodeTheme { Themes = themes });
            var mockController = _mock.Create<ThemesController>();

            var result = await mockController.Get(expectedId);

            var actionResult = Assert.IsType<ActionResult<ExtensionResource>>(result);
            var expectedResource = Assert.IsType<ExtensionResource>(actionResult.Value);
            Assert.Equal(expectedId, expectedResource.Id);
            Assert.Equal(expectedLabel, expectedResource.Themes[0].Label);
        }

        [Fact]
        public async Task Get_WithoutParams_ReturnsNotFoundObjectResultForNonexistentExtension()
        {
            const string nonExistentSessionId = "nonExistentSessionId_test";
            _mock.Mock<IExtensionsMetadataRepository>()
                .Setup(x => x.GetExtensionMetadata(nonExistentSessionId))
                .ReturnsAsync(() => null);
            var mockController = _mock.Create<ThemesController>();

            var result = await mockController.Get(nonExistentSessionId);

            var actionResult = Assert.IsType<ActionResult<ExtensionResource>>(result);
            var objectResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal(nonExistentSessionId, objectResult.Value);
        }

        [Theory]
        [InlineData(ExtensionType.NoThemes)]
        [InlineData(ExtensionType.NeedAttention)]
        public async Task Get_WithoutParams_ReturnsNotFoundObjectResultForExtensionWithNonDefaultType(
            ExtensionType extensionType)
        {
            const string expectedId = "expectedId_test";
            _mock.Mock<IExtensionsMetadataRepository>()
                .Setup(x => x.GetExtensionMetadata(expectedId))
                .ReturnsAsync(new ExtensionMetadata { Id = expectedId, Type = extensionType });
            var mockController = _mock.Create<ThemesController>();

            var result = await mockController.Get(expectedId);

            var actionResult = Assert.IsType<ActionResult<ExtensionResource>>(result);
            var objectResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal(expectedId, objectResult.Value);
        }

        [Fact]
        public async Task Get_WithoutParams_ReturnsStatus500_IfCouldNotFindTheme()
        {
            const string expectedId = "expectedId_test";

            _mock.Provide(CreateMapper());
            _mock.Mock<IExtensionsMetadataRepository>()
                .Setup(x => x.GetExtensionMetadata(expectedId))
                .ReturnsAsync(new ExtensionMetadata { Id = expectedId });
            _mock.Mock<IThemeRepository>()
                .Setup(x => x.GetTheme(expectedId))
                .ReturnsAsync(() => null);
            var mockController = _mock.Create<ThemesController>();

            var result = await mockController.Get(expectedId);

            var actionResult = Assert.IsType<ActionResult<ExtensionResource>>(result);
            var objectResult = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
