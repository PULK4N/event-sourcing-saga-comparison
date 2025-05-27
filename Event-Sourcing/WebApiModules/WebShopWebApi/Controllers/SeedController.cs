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
            "inventory-state-machine",
            invetoryCreated
        );

        var item1Id = Guid.NewGuid();
        var item2Id = Guid.NewGuid();

        var inventoryData = new InvetoryItemsAdded()
        {
            InventoryItems = new List<InventoryItem>()
            {
                new InventoryItem()
                {
                    Name = "TestItem1",
                    Id = item1Id,
                    Counter = 100
                },
                new InventoryItem()
                {
                    Name = "TestItem2",
                    Id = item2Id,
                    Counter = 200
                },
                new InventoryItem()
                {
                    Name = "TestItem3",
                    Id = Guid.NewGuid(),
                    Counter = 300
                }
            }
        };

        var addItemsToInventory = EventPayload.Create(
            Constants.EXECUTOR_ID,
            Constants.INVENTORY_ID,
            "inventory-state-machine",
            inventoryData
        );

        var createOrder = EventPayload.Create(
            Constants.EXECUTOR_ID,
            Constants.ORDER_ID,
            "order-state-machine",
            new OrderCreated()
        );

        var orderItemsAdded = EventPayload.Create(
            Constants.EXECUTOR_ID,
            Constants.ORDER_ID,
            "order-state-machine",
            new OrderItemAdded()
            {
                OrderItem = new OrderItem()
                {
                    Id = item1Id,
                    Price = 100,
                    Amount = 50
                }
            }
        );
        var orderItemsAdded2 = EventPayload.Create(
            Constants.EXECUTOR_ID,
            Constants.ORDER_ID,
            "order-state-machine",
            new OrderItemAdded()
            {
                OrderItem = new OrderItem()
                {
                    Id = item2Id,
                    Price = 200,
                    Amount = 200
                }
            }
        );

        events.AddRange(
            new List<EventPayload>()
            {
                createInventory,
                addItemsToInventory,
                createOrder,
                orderItemsAdded,
                orderItemsAdded2
            }
        );

        var result = await stateMachineHandler.ExecuteEvents(events.ToArray());
        return result;
    }
}
