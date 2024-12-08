using Microsoft.EntityFrameworkCore;

namespace SagaOrchestrator.Repositories
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<TransactionStep> TransactionSteps { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<TransactionStep>()
                .HasIndex(x => new { x.TransactionId, x.OrderNumber })
                .IsUnique(true);

            builder.Entity<Transaction>().HasIndex(x => new { x.Id }).IsUnique(true);
        }
    }
}
