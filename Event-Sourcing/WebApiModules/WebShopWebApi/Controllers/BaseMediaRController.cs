using ActionImplementations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebShopWebApi.Controllers;

public abstract class BaseMediaRController : ControllerBase
{
    private readonly IMediator _mediator;

    protected Task<T> Execute<T>(Command<T> command)
    {
        return _mediator.Send(command);
    }

    public BaseMediaRController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
