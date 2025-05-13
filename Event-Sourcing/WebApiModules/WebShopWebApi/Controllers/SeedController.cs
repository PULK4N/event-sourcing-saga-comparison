using EventSourcing.Core;
using EventSourcing.Shared.Models;
using InventoryModule.Events;
using InventoryModule.Models;
using Microsoft.AspNetCore.Mvc;

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
        var executor = Guid.Empty;
        var inventoryId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var events = new List<EventPayload>();

        var invetoryCreated = new InvetoryCreated() { Name = "Test Inventory", };

        var createInventory = EventPayload.Create(
            executor,
            inventoryId,
            "inventory-state-machine",
            invetoryCreated
        );

        var inventoryData = new InvetoryItemsAdded()
        {
            InventoryItems = new List<InventoryItem>()
            {
                new InventoryItem()
                {
                    Name = "TestItem1",
                    Id = Guid.NewGuid(),
                    Counter = 100
                },
                new InventoryItem()
                {
                    Name = "TestItem2",
                    Id = Guid.NewGuid(),
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
            executor,
            inventoryId,
            "inventory-state-machine",
            inventoryData
        );

        events.AddRange(new List<EventPayload>() { createInventory, addItemsToInventory });

        var result = await stateMachineHandler.ExecuteEvents(events.ToArray());
        return result;
    }
}
