namespace Ibero.Services.Utilitary.Persistence
{
    using Ibero.Services.Utilitary.Core.Entities;
    using Ibero.Services.Utilitary.Domain.Infrastructure.Abstract;
    using Microsoft.EntityFrameworkCore;

    public class UtilitaryDbContext : DbContext, IUtilitaryDbContext
    {
        public UtilitaryDbContext(DbContextOptions<UtilitaryDbContext> options)
            : base(options)
        {
        }

        
        public DbSet<State> Ibet_States { get; set; }
        public DbSet<DocumentType> Ibet_DocumentTypes { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UtilitaryDbContext).Assembly);
        }
    }
}
