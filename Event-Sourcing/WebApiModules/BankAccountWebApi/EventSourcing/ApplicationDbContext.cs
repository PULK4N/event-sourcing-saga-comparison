using BankAccountWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAccountWebApi.EventSourcing;

public class EventSourcingDbContext : DbContext
{
    public DbSet<SerializedEventPayload> SerializedEventPayload { get; set; }
    public DbSet<SerializedPayloadMessage> SerializedPayloadMessage { get; set; }
}
