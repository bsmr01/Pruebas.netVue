namespace Ibero.Services.Utilitary.Domain.Tests.Infrastructure
{
    using AutoMapper;
    using Ibero.Services.Utilitary.Persistence;
    using System;
    using Xunit;

    public class TestFixture : IDisposable
    {
        public UtilitaryDbContext Context { get; private set; }
        public IMapper Mapper { get; private set; }

        public TestFixture()
        {
            Context = UtilitaryDbContextFactory.Create();
        }
        public void Dispose()
        {
            UtilitaryDbContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<TestFixture> { }
}
