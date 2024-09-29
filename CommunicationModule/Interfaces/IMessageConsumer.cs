namespace CommunicationModule.Interfaces;

public interface IMessageConsumer<TKey, TValue>
{
    Task ConsumeAsync(string topic, Func<TKey, TValue, Task> messageHandler, CancellationToken cancellationToken);
}
