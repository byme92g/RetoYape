using SharedLib.DTOs;
using SharedLib.Enums;
using SharedLib.Messaging;

namespace AntiFraudService.AntiFraudService.Application.Handlers;

public class ValidateTransactionHandler : IKafkaConsumerHandler<FraudCheckDto>
{
    private readonly IKafkaProducer<FraudCheckResponseDto> _kafkaProducer;
    private readonly ILogger<ValidateTransactionHandler> _logger;

    public ValidateTransactionHandler(IKafkaProducer<FraudCheckResponseDto> kafkaProducer, ILogger<ValidateTransactionHandler> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }

    public async Task HandleMessageAsync(FraudCheckDto message)
    {
        _logger.LogInformation($"Validating transaction {message.TransactionExternalId} with amount {message.Value}");

        var result = message.Value > 2000 ? FraudStatus.Rejected : FraudStatus.Approved;

        var fraudResponse = new FraudCheckResponseDto(message.TransactionExternalId, result
        );

        await _kafkaProducer.ProduceAsync("antifraud-validation-response", fraudResponse);
    }
}
