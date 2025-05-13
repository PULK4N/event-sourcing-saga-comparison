using ActionImplementations;
using EventSourcing.Core;
using WebShopWebApi.Commands;
using WebShopWebApi.DTOs;

namespace WebShopWebApi.Handlers;

public class PlaceAnOrderHandler : CommandHanlder<PlaceOrder, OrderDTO>
{
    private readonly StateMachineHandler _stateMachineHandler;

    public PlaceAnOrderHandler(StateMachineHandler stateMachineHandler)
    {
        _stateMachineHandler = stateMachineHandler;
    }

    protected override async Task<OrderDTO> HandleInternal(
        PlaceOrder request,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
        await _stateMachineHandler.ExecuteEvents();
    }
}
