using BankAccountWebApi.Commands;
using BankAccountWebApi.EventSourcing;
using EventSourcing.Core;
using EventSourcing.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers().AddNewtonsoftJson();

        builder.Services.AddScoped<IEventStoreWithOutbox, EventStoreWithOutbox>();
        builder.Services.RegisterEventSourcingCoreInjection();
        builder.Services.AddScoped<SendMoneyCommand>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var connectionString = builder.Configuration.GetConnectionString("ApplicationDatabase");
        var contextOptions = new DbContextOptionsBuilder<EventSourcingDbContext>();
        builder
            .Services
            .AddDbContext<EventSourcingDbContext>(
                options => options.UseSqlServer(connectionString)
            );

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
