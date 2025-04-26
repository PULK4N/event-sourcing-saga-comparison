using EventSourcing.Models;

namespace EventSourcing.Core.Interfaces
{
    public interface ICommand
    {
        Task Execute(HookInfo hookInfo);
    }
}
