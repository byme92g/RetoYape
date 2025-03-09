using MediatR;
using Shared.DTOs;

namespace TransactionService.Transaction.Application.Commands;

public class CreateTransactionCommand(TransactionDto transaction) : IRequest<Guid>
{
    public TransactionDto Transaction { get; set; } = transaction;
}
