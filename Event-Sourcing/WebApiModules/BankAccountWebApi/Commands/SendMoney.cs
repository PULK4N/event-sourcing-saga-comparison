using Microsoft.AspNetCore.Mvc;

namespace BankAccountWebApi.Commands;

public class SendMoneyCommand
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    };

    private readonly ILogger<SendMoneyCommand> _logger;

    public SendMoneyCommand(ILogger<SendMoneyCommand> logger)
    {
        _logger = logger;
    }
}
