namespace Ibero.Services.Utilitary.Domain.Tests.Infrastructure
{
    using Ibero.Services.Utilitary.Persistence;
    using Microsoft.EntityFrameworkCore;
    using System;

    public class UtilitaryDbContextFactory
    {
        public static UtilitaryDbContext Create()
        {
            var options = new DbContextOptionsBuilder<UtilitaryDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new UtilitaryDbContext(options);

            context.Database.EnsureCreated();

            context.SaveChanges();

            return context;
        }

        public static void Destroy(UtilitaryDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}
