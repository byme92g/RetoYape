using Confluent.Kafka;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedLib.DTOs;
using SharedLib.Messaging;
using TransactionService.Transaction.Application.Handlers;
using TransactionService.Transaction.Application.Validators;
using TransactionService.Transaction.Domain.Interfaces;
using TransactionService.Transaction.Infraestructure.Repositories;

namespace TransactionService.Transaction.Infraestructure;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration cfg)
    {
        AddDatabase(services, cfg);
        AddKafka(services, cfg);
        AddRepositories(services);
        AddHandlers(services);

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TransactionDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), o => o.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null
            ))
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information);
        });

        return services;
    }

    private static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"],
            EnableIdempotence = true,
            Acks = Acks.All
        };

        services.AddSingleton(producerConfig);
        services.AddSingleton<IKafkaProducer<TransactionPostDto>, KafkaProducer<TransactionPostDto>>();
        services.AddSingleton<IKafkaProducer<FraudCheckDto>, KafkaProducer<FraudCheckDto>>();

        services.AddSingleton(sp =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = configuration["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            return new ConsumerBuilder<Ignore, string>(config).Build();
        });

        services.AddHostedService<KafkaConsumer<FraudCheckResponseDto>>();
        services.AddScoped<IKafkaConsumerHandler<FraudCheckResponseDto>, UpdateTransactionHandler>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CreateTransactionHandler).Assembly));

        services.AddValidatorsFromAssemblyContaining<CreateTransactionCommandValidator>();

        return services;
    }
}