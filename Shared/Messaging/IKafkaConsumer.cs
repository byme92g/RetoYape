namespace Shared.Messaging;

public interface IKafkaConsumer
{
    Task ConsumeAsync(CancellationToken ct);
}
