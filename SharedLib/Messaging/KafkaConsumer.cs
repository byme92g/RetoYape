using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedLib.Messaging;

public class KafkaConsumer<T> : BackgroundService where T : class
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly ILogger<KafkaConsumer<T>> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly string _topic;

    public KafkaConsumer(
        IConsumer<Ignore, string> consumer,
        ILogger<KafkaConsumer<T>> logger,
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration)
    {
        _consumer = consumer;
        _logger = logger;
        _scopeFactory = scopeFactory;
        _topic = configuration["Kafka:Topic"] ?? "default-topic";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topic);
        _logger.LogInformation($"Kafka Consumer escuchando el topic: {_topic}");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                _logger.LogInformation($"Mensaje consumido '{consumeResult.Message.Value}'");

                using var scope = _scopeFactory.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IKafkaConsumerHandler<T>>();

                var message = JsonConvert.DeserializeObject<T>(consumeResult.Message.Value);
                if (message != null)
                {
                    await handler.HandleMessageAsync(message);
                }

                _consumer.Commit(consumeResult);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kafka consumption error");
        }
        finally
        {
            _consumer.Close();
        }
    }
}
