using CommunicationModule;
using CommunicationModule.Config;
using CommunicationModule.Interfaces;
using EventSourcing.Core;
using EventSourcing.Persistence;
using EventSourcing.Shared.Models;
using InventoryModule;
using OrderModule;
using PaymentModule;
using ShippingModule;
using WebShopWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var b = new InventoryStateData();
var c = new OrderStateData();
var d = new PaymentStateData();
var l = new ShipmentStateData();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.RegisterEventSourcingPersistence(builder.Configuration);
builder.Services.RegisterEventSourcingCoreInjection();
builder.Services.AddHostedService<OutboxBackgroundService>();

var kafkaConfig = builder.Configuration.GetSection("KafkaProducerConfig");
builder.Services.Configure<KafkaProducerConfig>(kafkaConfig);
builder
    .Services
    .AddSingleton<IMessageProducer<ISharedStateData>, KafkaProducer<ISharedStateData>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
