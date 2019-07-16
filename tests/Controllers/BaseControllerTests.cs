using System;
using Autofac.Extras.Moq;
using AutoMapper;
using VSCThemesStore.WebApi.Mapping;

namespace VSCThemesStore.WebApi.Tests.Controllers
{
    public abstract class BaseControllerTests : IDisposable
    {
        protected readonly AutoMock _mock;

        protected BaseControllerTests()
        {
            _mock = AutoMock.GetLoose();
        }

        public void Dispose()
        {
            _mock.Dispose();
        }

        protected IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            return config.CreateMapper();
        }
    }
}
