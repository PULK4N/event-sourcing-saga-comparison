using Microsoft.EntityFrameworkCore;
using OrderModule.Models;

namespace OrderImplementationModule;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Order> Orders { get; set; }
}
