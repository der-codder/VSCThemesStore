using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VSCThemesStore.WebApi.Controllers;
using VSCThemesStore.WebApi.Controllers.Resources;
using VSCThemesStore.WebApi.Domain.Models;
using VSCThemesStore.WebApi.Domain.Repositories;
using VSCThemesStore.WebApi.Mapping;
using Xunit;

namespace VSCThemesStore.WebApi.Tests.Controllers
{
    public class MetadataControllerTests : IDisposable
    {
        private readonly AutoMock _mock;

        public MetadataControllerTests()
        {
            _mock = AutoMock.GetLoose();
        }

        public void Dispose()
        {
            _mock.Dispose();
        }

        [Fact]
        public async Task Get_ReturnsProperMetadata()
        {
            const string expectedId = "expectedId_test";
            _mock.Provide(CreateMapper());
            _mock.Mock<IExtensionsMetadataRepository>()
                .Setup(x => x.GetExtensionMetadata(expectedId))
                .ReturnsAsync(new ExtensionMetadata { Id = expectedId });
            var mockController = _mock.Create<MetadataController>();

            var result = await mockController.Get(expectedId);

            var actionResult = Assert.IsType<ActionResult<ExtensionMetadataResource>>(result);
            var returnValue = Assert.IsType<ExtensionMetadataResource>(actionResult.Value);
            Assert.Equal(expectedId, returnValue.Id);
        }

        [Fact]
        public async Task Get_ReturnsNotFoundObjectResultForNonexistentExtension()
        {
            const string nonExistentSessionId = "nonExistentSessionId_test";
            _mock.Mock<IExtensionsMetadataRepository>()
                .Setup(x => x.GetExtensionMetadata(nonExistentSessionId))
                .ReturnsAsync(() => null);
            var mockController = _mock.Create<MetadataController>();

            var result = await mockController.Get(nonExistentSessionId);

            var actionResult = Assert.IsType<ActionResult<ExtensionMetadataResource>>(result);
            var objectResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal(nonExistentSessionId, objectResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsNotFoundObjectResultForExtensionWith_NoThemes_Type()
        {
            const string expectedId = "expectedId_test";
            _mock.Mock<IExtensionsMetadataRepository>()
                .Setup(x => x.GetExtensionMetadata(expectedId))
                .ReturnsAsync(new ExtensionMetadata { Id = expectedId, Type = ExtensionType.NoThemes });
            var mockController = _mock.Create<MetadataController>();

            var result = await mockController.Get(expectedId);

            var actionResult = Assert.IsType<ActionResult<ExtensionMetadataResource>>(result);
            var objectResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal(expectedId, objectResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsNotFoundObjectResultForExtensionWith_NeedAttention_Type()
        {
            const string expectedId = "expectedId_test";
            _mock.Mock<IExtensionsMetadataRepository>()
                .Setup(x => x.GetExtensionMetadata(expectedId))
                .ReturnsAsync(new ExtensionMetadata { Id = expectedId, Type = ExtensionType.NeedAttention });
            var mockController = _mock.Create<MetadataController>();

            var result = await mockController.Get(expectedId);

            var actionResult = Assert.IsType<ActionResult<ExtensionMetadataResource>>(result);
            var objectResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal(expectedId, objectResult.Value);
        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            return config.CreateMapper();
        }
    }
}
