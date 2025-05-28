using Microsoft.EntityFrameworkCore;
using ShippingModule;

namespace ShippingImplementationModule;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<ShipmentStateData> Shipments { get; set; }
}
