﻿using SharedLib.DTOs;
using SharedLib.Enums;
using SharedLib.Messaging;
using TransactionService.Transaction.Domain.Interfaces;

namespace TransactionService.Transaction.Application.Handlers;

public class UpdateTransactionHandler : IKafkaConsumerHandler<FraudCheckResponseDto>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<UpdateTransactionHandler> _logger;

    public UpdateTransactionHandler(ITransactionRepository transactionRepository, ILogger<UpdateTransactionHandler> logger)
    {
        _transactionRepository = transactionRepository;
        _logger = logger;
    }

    public async Task HandleMessageAsync(FraudCheckResponseDto message)
    {
        try
        {
            _logger.LogInformation($"Mensaje: {message}");

            var transaction = await _transactionRepository.GetByExternalIdAsync(message.TransactionExternalId);

            if (transaction is not null)
            {
                transaction.Status = message.Result == FraudStatus.Approved ? TransactionStatus.Approved : TransactionStatus.Rejected;

                await _transactionRepository.UpdateAsync(transaction);

                _logger.LogInformation($"Transaccion extId: {transaction.TransactionExternalId} actualizada a {transaction.Status}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling message");
        }
    }
}
