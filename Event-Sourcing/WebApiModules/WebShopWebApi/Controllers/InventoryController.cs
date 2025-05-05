using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebShopWebApi.Commands;
using WebShopWebApi.Controllers;
using WebShopWebApi.DTOs;

[ApiController]
[Route("[controller]")]
public class InventoryController : BaseMediaRController
{
    public InventoryController(IMediator mediator)
        : base(mediator) { }

    [HttpPost("create-inventory")]
    public async Task<TestDTO> CreateInventory([FromBody] TestCommand testCommand)
    {
        return await Execute<TestDTO>(testCommand);
    }
}
