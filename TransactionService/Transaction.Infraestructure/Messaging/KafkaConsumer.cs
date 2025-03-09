using Confluent.Kafka;
using Shared.Messaging;

namespace TransactionService.Transaction.Infraestructure.Messaging;

public class KafkaConsumer : BackgroundService, IKafkaConsumer
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly IKafkaConsumerHandler<string> _handler;
    private readonly ILogger<KafkaConsumer> _logger;

    public KafkaConsumer(IConfiguration configuration, IKafkaConsumerHandler<string> handler, ILogger<KafkaConsumer> logger)
    {
        _handler = handler;
        _logger = logger;

        var config = new ConsumerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"],
            GroupId = configuration["Kafka:GroupId"],
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        _consumer.Subscribe(configuration["Kafka:Topic"]);
    }

    public async Task ConsumeAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(ct);
                _logger.LogInformation($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");

                await _handler.HandleMessageAsync(consumeResult.Message.Value);
                _consumer.Commit(consumeResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming message");
            }
        }
    }

    protected override Task ExecuteAsync(CancellationToken ct) => ConsumeAsync(ct);
}
