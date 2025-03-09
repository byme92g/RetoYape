namespace Shared.Messaging;

public interface IKafkaConsumerHandler<T>
{
    Task HandleMessageAsync(T message);
}