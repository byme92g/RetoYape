using MediatR;
using Shared.DTOs;
using Shared.Messaging;
using TransactionService.Transaction.Application.Commands;
using TransactionService.Transaction.Domain.Entities;
using TransactionService.Transaction.Domain.Interfaces;

namespace TransactionService.Transaction.Application.Handlers;

public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, Guid>
{
    private readonly ITransactionRepository _repository;
    private readonly IKafkaProducer<FraudCheckDto> _kafkaProducer;
    private readonly ILogger<CreateTransactionHandler> _logger;

    public CreateTransactionHandler(ITransactionRepository transactionRepository, IKafkaProducer<FraudCheckDto> kafkaProducer, ILogger<CreateTransactionHandler> logger)
    {
        _repository = transactionRepository;
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Handling message: {request}");

            var transaction = new FinancialTransaction
            {
                SourceAccountId = request.SourceAccountId,
                TargetAccountId = request.TargetAccountId,
                TransferTypeId = request.TransferTypeId,
                Value = request.Value
            };

            await _repository.AddAsync(transaction);

            var fraudCheckDto = new FraudCheckDto
            {
                TransactionExternalId = transaction.TransactionExternalId,
                Value = transaction.Value
            };

            await _kafkaProducer.ProduceAsync("anti-fraud-validation", fraudCheckDto);
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
