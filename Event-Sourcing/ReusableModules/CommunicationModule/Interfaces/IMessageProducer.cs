namespace CommunicationModule.Interfaces;

public interface IMessageProducer<TValue>
{
    Task ProduceAsync(string topic, string key, TValue value);
}
