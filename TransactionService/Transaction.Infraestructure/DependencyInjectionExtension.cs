using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Messaging;
using TransactionService.Transaction.Application.Handlers;
using TransactionService.Transaction.Domain.Interfaces;
using TransactionService.Transaction.Infraestructure.Repositories;

namespace TransactionService.Transaction.Infraestructure;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDatabase(services, configuration);
        AddKafka(services);
        AddRepositories(services);
        AddHandlers(services);

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TransactionDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        });

        return services;
    }

    private static IServiceCollection AddKafka(this IServiceCollection services)
    {
        services.AddSingleton<IKafkaProducer<TransactionPostDto>, KafkaProducer<TransactionPostDto>>();
        services.AddSingleton<IKafkaProducer<FraudCheckDto>, KafkaProducer<FraudCheckDto>>();

        services.AddScoped<IKafkaConsumerHandler<FraudCheckResponseDto>, UpdateTransactionHandler>();
        services.AddHostedService<KafkaConsumer<FraudCheckResponseDto>>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddMediatR(
            config => config.RegisterServicesFromAssembly(
                typeof(CreateTransactionHandler).Assembly));

        return services;
    }
}
