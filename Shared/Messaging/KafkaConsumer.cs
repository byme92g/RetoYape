using Confluent.Kafka;
using Newtonsoft.Json;

namespace Shared.Messaging;

public class KafkaConsumer<T> : BackgroundService, IKafkaConsumer<T> where T : class
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly IKafkaConsumerHandler<T> _handler;
    private readonly ILogger<KafkaConsumer<T>> _logger;
    //private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string _topic;

    public KafkaConsumer(
        IConfiguration configuration,
        IKafkaConsumerHandler<T> handler, ILogger<KafkaConsumer<T>> logger,
        //IServiceScopeFactory serviceScopeFactory,
        string topic)
    {
        _handler = handler;
        _logger = logger;
        _topic = topic;
        //_serviceScopeFactory = serviceScopeFactory;

        var config = new ConsumerConfig
        {
            BootstrapServers = "kafka:9092",
            GroupId = $"consumer-group-{typeof(T).Name}",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        _consumer.Subscribe(_topic);
        //_serviceScopeFactory = serviceScopeFactory;
    }

    public async Task ConsumeAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                //using var scope = _serviceScopeFactory.CreateScope();

                var consumeResult = _consumer.Consume(ct);
                _logger.LogInformation($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");

                var message = JsonConvert.DeserializeObject<T>(consumeResult.Message.Value);

                if (message is not null)
                {
                    await _handler.HandleMessageAsync(message);
                }

                _consumer.Commit(consumeResult);
                await Task.Delay(1000, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming message");
            }
        }
    }

    protected override Task ExecuteAsync(CancellationToken ct) => ConsumeAsync(ct);
}