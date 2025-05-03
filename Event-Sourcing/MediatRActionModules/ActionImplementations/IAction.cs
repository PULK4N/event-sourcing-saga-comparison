using ActionModels;
using MediatR;

namespace ActionImplementations;

public interface IAction<T> : IRequest<T>
{
    public Executor Executor { get; set; }
}
