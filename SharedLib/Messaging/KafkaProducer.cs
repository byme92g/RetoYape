using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SharedLib.Messaging;
public class KafkaProducer<T> : IKafkaProducer<T> where T : class
{
    private readonly IProducer<Null, string> _producer;
    private readonly ILogger<KafkaProducer<T>> _logger;

    public KafkaProducer(ProducerConfig config, ILogger<KafkaProducer<T>> logger)
    {
        _producer = new ProducerBuilder<Null, string>(config).Build();
        _logger = logger;
    }

    public async Task ProduceAsync(string topic, T message)
    {
        _logger.LogInformation("Produciendo mensaje: {Message}", message);
        var jsonMsg = JsonConvert.SerializeObject(message);
        await _producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMsg });
    }
}