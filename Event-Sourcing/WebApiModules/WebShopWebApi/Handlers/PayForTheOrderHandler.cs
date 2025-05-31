using ActionImplementations;
using EventSourcing.Core;
using EventSourcing.Shared.Models;
using InventoryModule.Events;
using OrderModule;
using OrderModule.Events;
using PaymentModule.Events;
using ShippingModule.Events;
using WebShopWebApi.Commands;
using WebShopWebApi.DTOs;

namespace WebShopWebApi.Handlers;

public class PayForOrderHandler : CommandHanlder<PlaceOrder, OrderDTO>
{
    private readonly StateMachineHandler _stateMachineHandler;

    public PayForOrderHandler(StateMachineHandler stateMachineHandler)
    {
        _stateMachineHandler = stateMachineHandler;
    }

    protected override async Task<OrderDTO> HandleInternal(
        PlaceOrder request,
        CancellationToken cancellationToken
    )
    {
        var orderSuccessful = EventPayload.Create(
            Constants.EXECUTOR_ID,
            Constants.ORDER_ID,
            Constants.ORDER_STATE_MACHINE,
            new OrderSuccessful()
        );

        var invetoryItemsOrdered = EventPayload.Create(
            Constants.EXECUTOR_ID,
            Constants.INVENTORY_ID,
            Constants.INVENTORY_STATE_MACHINE,
            new InvetoryItemsOrdered() { OrderId = Constants.ORDER_ID }
        );

        var paymentSuccessful = EventPayload.Create(
            Constants.EXECUTOR_ID,
            Constants.PAYMENT_ID,
            Constants.PAYMENT_STATE_MACHINE,
            new PaymentSuccessful() { }
        );

        var shipmentStarted = EventPayload.Create(
            Constants.EXECUTOR_ID,
            Constants.SHIPMENT_ID,
            Constants.SHIPMENT_STATE_MACHINE,
            new ShipmentCreated() { }
        );

        var result = await _stateMachineHandler.ExecuteEvents(
            orderSuccessful,
            invetoryItemsOrdered,
            paymentSuccessful,
            shipmentStarted
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
