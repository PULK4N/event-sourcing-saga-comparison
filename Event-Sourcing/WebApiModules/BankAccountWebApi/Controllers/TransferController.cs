using BankAccountWebApi.Commands;
using Microsoft.AspNetCore.Mvc;

namespace BankAccountWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TransferController : ControllerBase
{
    private readonly ILogger<TransferController> _logger;

    public TransferController(ILogger<TransferController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "transfer-money")]
    public async Task<object> Get(
        [FromBody] SendMoneyCommand body,
        [FromServices] SendMoneyCommand command
    )
    {
        command.MoneyToTransfer = body.MoneyToTransfer;
        command.CurrentUser = body.CurrentUser;
        command.UserSentTo = body.UserSentTo;

        return await command.ExecuteInternal();
    }
}
