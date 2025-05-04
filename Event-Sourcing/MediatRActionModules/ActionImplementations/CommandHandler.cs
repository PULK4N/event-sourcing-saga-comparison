using ActionModels;
using MediatR;

namespace ActionImplementations;

public abstract class Command<T> : IRequest<T>
{
    public Executor Executor { get; set; } = new Executor();
};

public abstract class CommandHanlder<T, ReturnT> : IRequestHandler<T, ReturnT>
    where T : Command<ReturnT>
{
    public virtual async Task<ReturnT> Handle(T request, CancellationToken cancellationToken)
    {
        return await HandleInternal(request, cancellationToken);
    }

    protected abstract Task<ReturnT> HandleInternal(T request, CancellationToken cancellationToken);
}
