using InventoryImplementationModule.Models;
using InventoryModule;
using Microsoft.EntityFrameworkCore;

namespace InventoryImplementationModule;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Inventory> Inventories { get; set; }
}
