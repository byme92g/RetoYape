using MediatR;
using SharedLib.Enums;

namespace TransactionService.Transaction.Application.Commands;

public class CreateTransactionCommand : IRequest<Guid>
{
    public Guid SourceAccountId { get; set; }
    public Guid TargetAccountId { get; set; }
    public TransferType TransferTypeId { get; set; }
    public decimal Value { get; set; }
}
