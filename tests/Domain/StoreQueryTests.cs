using System;
using VSCThemesStore.WebApi.Domain.Models;
using Xunit;

namespace VSCThemesStore.WebApi.Tests.Domain
{
    public class StoreQueryTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData(1, -1)]
        [InlineData(1, 0)]
        [InlineData(0, 0)]
        [InlineData(-1, 0)]
        public void NormalizeQueryParams_NormalizingIrrelevantPageNumber_And_PageSize(
            int? pageNumber,
            int? pageSize)
        {
            var query = new StoreQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            query.NormalizeQueryParams();

            Assert.Equal(1, query.PageNumber);
            Assert.Equal(50, query.PageSize);
        }

        [Fact]
        public void NormalizeQueryParams_DoesNotChangeValidPageNumber()
        {
            const int expectedPageNumber = 2;
            var query = new StoreQuery
            {
                PageNumber = expectedPageNumber
            };

            query.NormalizeQueryParams();

            Assert.Equal(expectedPageNumber, query.PageNumber);
        }

        [Fact]
        public void NormalizeQueryParams_DoesNotChangeValidPageSize()
        {
            const int expectedPageSize = 20;
            var query = new StoreQuery
            {
                PageSize = expectedPageSize
            };

            query.NormalizeQueryParams();

            Assert.Equal(expectedPageSize, query.PageSize);
        }
    }
}
