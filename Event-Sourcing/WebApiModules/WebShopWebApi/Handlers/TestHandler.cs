using ActionImplementations;
using WebShopWebApi.Commands;
using WebShopWebApi.DTOs;

namespace WebShopWebApi.Handlers;

public class TestHandler : CommandHanlder<TestCommand, TestDTO>
{
    protected override Task<TestDTO> HandleInternal(
        TestCommand request,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new TestDTO());
    }
}
