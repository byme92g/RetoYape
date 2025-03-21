namespace SharedLib.Messaging;

public interface IKafkaProducer<T>
{
    Task ProduceAsync(string topic, T message);
}
