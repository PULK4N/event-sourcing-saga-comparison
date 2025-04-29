using BankAccountWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAccountWebApi.EventSourcing;

public class EventSourcingDbContext : DbContext
{
    public EventSourcingDbContext(DbContextOptions<EventSourcingDbContext> options)
        : base(options) { }

    public DbSet<SerializedEventPayload> SerializedEventPayload { get; set; }
    public DbSet<SerializedPayloadMessage> SerializedPayloadMessage { get; set; }
}
