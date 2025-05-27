using CommunicationModule;
using CommunicationModule.Config;
using CommunicationModule.Interfaces;
using Microsoft.EntityFrameworkCore;
using ShippingImplementationModule;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<ShipmentRepository>();
        builder
            .Services
            .Configure<KafkaConsumerConfig>(
                builder.Configuration.GetSection("KafkaConsumerConfig")
            );
        builder
            .Services
            .AddSingleton<IMessageConsumer<string, string>, KafkaConsumer<string, string>>();
        builder.Services.AddHostedService<KafkaMessageReceiverBackgroundService>();

        var connectionString = builder.Configuration.GetConnectionString("ApplicationDatabase");
        var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder
            .Services
            .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

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
