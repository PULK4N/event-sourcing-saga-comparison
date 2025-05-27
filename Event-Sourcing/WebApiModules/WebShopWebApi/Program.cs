using EventSourcing.Core;
using EventSourcing.Persistence;
using InventoryModule;
using OrderModule;
using PaymentModule;
using ShippingModule;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var a = new InventoryModule.Events.InvetoryCreated();
var b = new InventoryStateData();
var c = new OrderStateData();
var d = new PaymentStateData();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.RegisterEventSourcingPersistence(builder.Configuration);
builder.Services.RegisterEventSourcingCoreInjection();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
