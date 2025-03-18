using Confluent.Kafka;
using Newtonsoft.Json;

namespace Shared.Messaging;

public class KafkaProducer<T> : IKafkaProducer<T> where T : class
{
    private readonly IProducer<Null, string> _producer;
    private readonly ILogger<KafkaProducer<T>> _logger;

    public KafkaProducer(IConfiguration configuration, ILogger<KafkaProducer<T>> logger)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "kafka:9092",
            EnableIdempotence = true,
            Acks = Acks.All
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
        _logger = logger;
    }

    public async Task ProduceAsync(string topic, T message)
    {
        _logger.LogInformation("Producing message: {Message}", message);
        var jsonMsg = JsonConvert.SerializeObject(message);
        await _producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMsg });
    }
}