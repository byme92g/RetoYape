﻿using SharedLib.DTOs;
using TransactionService.Transaction.Domain.Entities;

namespace TransactionService.Transaction.Application.Mappers;

public static class TransactionMapper
{
    public static TransactionReadDto ToReadDto(this FinancialTransaction entity)
    {
        return new TransactionReadDto
        {
            TransactionExternalId = entity.TransactionExternalId,
            SourceAccountId = entity.SourceAccountId,
            TargetAccountId = entity.TargetAccountId,
            TransferType = entity.TransferTypeId.ToString(),
            Value = entity.Value,
            CreatedAt = entity.CreatedAt,
            Status = entity.Status.ToString()
        };
    }

    public static List<TransactionReadDto> ToReadDto(this List<FinancialTransaction> entities) => entities.Select(ToReadDto).ToList();
}