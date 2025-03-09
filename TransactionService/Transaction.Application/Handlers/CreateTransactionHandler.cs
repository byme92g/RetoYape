using MediatR;
using Shared.DTOs;
using Shared.Messaging;
using TransactionService.Transaction.Application.Commands;
using TransactionService.Transaction.Domain.Interfaces;

namespace TransactionService.Transaction.Application.Handlers;

public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, Guid>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IKafkaProducer _kafkaProducer;
    private readonly ILogger _logger;

    public CreateTransactionHandler(ITransactionRepository transactionRepository, IKafkaProducer kafkaProducer, ILogger logger)
    {
        _transactionRepository = transactionRepository;
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Handling message: {request}");
            var transaction = new Domain.Entities.Transaction
            {
                SourceAccountId = request.Transaction.SourceAccountId,
                TargetAccountId = request.Transaction.TargetAccountId,
                TransferTypeId = request.Transaction.TransferTypeId,
                Value = request.Transaction.Value
            };

            await _transactionRepository.AddAsync(transaction);

            var fraudCheckDto = new FraudCheckDto
            {
                TransactionExternalId = transaction.TransactionExternalId,
                Value = transaction.Value
            };

            await _kafkaProducer.SendMessageAsync("anti-fraud-validation", fraudCheckDto);
            _logger.LogInformation($"Transaction {transaction.TransactionExternalId} created");

            return transaction.TransactionExternalId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling message");
            throw;
        }
    }
}
