using SharedLib.DTOs;
using SharedLib.Enums;
using SharedLib.Messaging;

namespace AntiFraudService.AntiFraudService.Application.Handlers;

public class ValidateTransactionHandler : IKafkaConsumerHandler<FraudCheckDto>
{
    private readonly IKafkaProducer<FraudCheckResponseDto> _kafkaProducer;
    private readonly ILogger<ValidateTransactionHandler> _logger;

    private static decimal _totalAccumulated = 0;

    public ValidateTransactionHandler(IKafkaProducer<FraudCheckResponseDto> kafkaProducer, ILogger<ValidateTransactionHandler> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }

    public async Task HandleMessageAsync(FraudCheckDto message)
    {
        _logger.LogInformation($"Validando transaccion extId: {message.TransactionExternalId} monto: {message.Value}");
        FraudStatus result;

        if (message.Value > 2000) result = FraudStatus.Rejected;

        else if (_totalAccumulated + message.Value > 20000) result = FraudStatus.Rejected;

        else
        {
            result = FraudStatus.Approved;
            _totalAccumulated += message.Value;
        }


        var fraudResponse = new FraudCheckResponseDto(message.TransactionExternalId, result
        );

        await _kafkaProducer.ProduceAsync("antifraud-validation-response", fraudResponse);
    }
}
