using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebShopWebApi.Commands;
using WebShopWebApi.Controllers;
using WebShopWebApi.DTOs;

[ApiController]
[Route("[controller]")]
public class OrderController : BaseMediaRController
{
    public OrderController(IMediator mediator)
        : base(mediator) { }

    [HttpPost("place-order")]
    public async Task<OrderDTO> PlaceOrder([FromBody] PlaceOrder command)
    {
        return await Execute<OrderDTO>(command);
    }
    [HttpPost("pay-order")]
    public async Task<OrderDTO> PayOrder([FromBody] PayForTheOrder command)
    {
        return await Execute<OrderDTO>(command);
    }
}
