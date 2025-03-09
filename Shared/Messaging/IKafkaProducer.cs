namespace Shared.Messaging;

public interface IKafkaProducer
{
    Task SendMessageAsync<T>(string topic, T message);
}
