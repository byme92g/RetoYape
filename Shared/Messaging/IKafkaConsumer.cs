namespace Shared.Messaging;

public interface IKafkaConsumer<T>
{
    Task ConsumeAsync(CancellationToken ct);
}
