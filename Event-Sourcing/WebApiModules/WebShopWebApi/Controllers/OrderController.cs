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
    public async Task<TestDTO> PlaceOrder([FromBody] TestCommand testCommand)
    {
        return await Execute<TestDTO>(testCommand);
    }
}
