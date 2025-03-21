namespace SharedLib.Messaging;

public interface IKafkaConsumerHandler<T>
{
    Task HandleMessageAsync(T message);
}