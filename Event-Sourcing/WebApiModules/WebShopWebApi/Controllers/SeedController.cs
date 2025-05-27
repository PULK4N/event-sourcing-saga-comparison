using EventSourcing.Core;
using EventSourcing.Shared.Models;
using InventoryModule.Events;
using InventoryModule.Models;
using Microsoft.AspNetCore.Mvc;
using OrderModule.Events;
using OrderModule.Models;

namespace WebShopWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SeedController : ControllerBase
{
    public SeedController() { }

    // Treba nam da izvrsimo place order i pay, sve ostalo da imamo vec
    //
    // 1. Treba da kreiramo inventory
    // 2. Treba da dodamo iteme u inventory
    // 3. Treba da kreiramo order
    // 4. Treba da dodamo iteme u order
    //

    [HttpPost]
    public async Task<object> SeedItems([FromServices] StateMachineHandler stateMachineHandler)
    {
        var events = new List<EventPayload>();

        var invetoryCreated = new InvetoryCreated() { Name = "Test Inventory", };

        var createInventory = EventPayload.Create(
            Constants.EXECUTOR_ID,
            Constants.INVENTORY_ID,
            Constants.INVENTORY_STATE_MACHINE,
            invetoryCreated
        );

        var item1Id = Guid.NewGuid();
        var item2Id = Guid.NewGuid();

        var inventoryItemsAdded = new InvetoryItemsAdded()
        {
            InventoryItems = new List<InventoryItem>()
            {
                new InventoryItem()
                {
                    Name = "Laptop",
                    Id = item1Id,
                    Counter = 100
                },
                new InventoryItem()
                {
                    Name = "Keyboard",
                    Id = item2Id,
                    Counter = 200
                },
                new InventoryItem()
                {
                    Name = "Random",
                    Id = Guid.NewGuid(),
                    Counter = 300
                }
            }
        };

        var addItemsToInventory = EventPayload.Create(
            Constants.EXECUTOR_ID,
            Constants.INVENTORY_ID,
            Constants.INVENTORY_STATE_MACHINE,
            inventoryItemsAdded
        );

        var createOrder = EventPayload.Create(
            Constants.EXECUTOR_ID,
            Constants.ORDER_ID,
            Constants.ORDER_STATE_MACHINE,
            new OrderCreated()
        );

        var orderItemsAdded = EventPayload.Create(
            Constants.EXECUTOR_ID,
            Constants.ORDER_ID,
            Constants.ORDER_STATE_MACHINE,
            new OrderItemAdded()
            {
                OrderItem = new OrderItem()
                {
                    Id = item1Id,
                    Name = "Laptop",
                    Price = 100,
                    Amount = 50
                }
            }
        );
        var orderItemsAdded2 = EventPayload.Create(
            Constants.EXECUTOR_ID,
            Constants.ORDER_ID,
            Constants.ORDER_STATE_MACHINE,
            new OrderItemAdded()
            {
                OrderItem = new OrderItem()
                {
                    Id = item2Id,
                    Name = "Keyboard",
                    Price = 200,
                    Amount = 200
                }
            }
        );

        var inventoryReserved = EventPayload.Create(
            Constants.EXECUTOR_ID,
            Constants.INVENTORY_ID,
            Constants.INVENTORY_STATE_MACHINE,
            new InvetoryItemsReserved()
            {
                OrderId = Constants.ORDER_ID,
                ReservedItems = new List<InventoryItem>()
                {
                    new InventoryItem() { Id = item1Id, Counter = 50 },
                    new InventoryItem() { Id = item2Id, Counter = 200 }
                }
            }
        );

        events.AddRange(
            new List<EventPayload>()
            {
                createOrder,
                orderItemsAdded,
                orderItemsAdded2,
                createInventory,
                addItemsToInventory,
                inventoryReserved
            }
        );

        var result = await stateMachineHandler.ExecuteEvents(events.ToArray());
        return result;
    }
}
