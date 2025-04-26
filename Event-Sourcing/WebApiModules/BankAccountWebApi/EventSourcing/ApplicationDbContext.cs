using EventSourcing.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAccountWebApi.Controllers;

public class EventSourcingDbContext : DbContext
{
    public DbSet<SerializedEventPayload> SerializedEventPayload { get; set; }
    public DbSet<SerializedPayloadMessage> SerializedPayloadMessage { get; set; }
}
