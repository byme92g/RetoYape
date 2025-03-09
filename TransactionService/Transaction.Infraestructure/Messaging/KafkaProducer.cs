using Confluent.Kafka;
using Newtonsoft.Json;
using Shared.Messaging;

namespace TransactionService.Transaction.Infraestructure.Messaging;

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducer()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            EnableIdempotence = true,
            Acks = Acks.All
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task SendMessageAsync<T>(string topic, T message)
    {
        var jsonMsg = JsonConvert.SerializeObject(message);
        await _producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMsg });
    }
}
