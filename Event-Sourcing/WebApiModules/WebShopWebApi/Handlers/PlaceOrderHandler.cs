using ActionImplementations;
using EventSourcing.Core;
using EventSourcing.Shared.Models;
using InventoryModule.Events;
using OrderModule;
using OrderModule.Events;
using PaymentModule.Events;
using WebShopWebApi.Commands;
using WebShopWebApi.DTOs;

namespace WebShopWebApi.Handlers;

public class PlaceOrderHandler : CommandHanlder<PlaceOrder, OrderDTO>
{
    private readonly StateMachineHandler _stateMachineHandler;

    public PlaceOrderHandler(StateMachineHandler stateMachineHandler)
    {
        _stateMachineHandler = stateMachineHandler;
    }

    protected override async Task<OrderDTO> HandleInternal(
        PlaceOrder request,
        CancellationToken cancellationToken
    )
    {
        var orderPlacedPayload = EventPayload.Create(
            Constants.EXECUTOR_ID,
            Constants.ORDER_ID,
            Constants.ORDER_STATE_MACHINE,
            new OrderPlaced()
        );

        var inventoryItemsOrdered = EventPayload.Create(
            Constants.EXECUTOR_ID,
            Constants.INVENTORY_ID,
            Constants.INVENTORY_STATE_MACHINE,
            new InvetoryItemsOrdered() { OrderId = Constants.ORDER_ID }
        );

        var paymentStarted = EventPayload.Create(
            Constants.EXECUTOR_ID,
            Constants.PAYMENT_ID,
            Constants.PAYMENT_STATE_MACHINE,
            new PaymentInitiated() { OrderId = Constants.ORDER_ID, Amount = 200 * 200 + 50 * 100 }
        );

        var result = await _stateMachineHandler.ExecuteEvents(
            orderPlacedPayload,
            inventoryItemsOrdered,
            paymentStarted
        );
        var orderStateData = (OrderStateData)result[Constants.ORDER_ID].StateData;

        return new OrderDTO()
        {
            Id = Constants.ORDER_ID,
            Items = orderStateData.Items,
            Status = orderStateData.Status
        };
    }
}
