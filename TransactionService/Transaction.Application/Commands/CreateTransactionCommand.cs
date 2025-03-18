using MediatR;

namespace TransactionService.Transaction.Application.Commands;

public class CreateTransactionCommand : IRequest<Guid>
{
    public Guid SourceAccountId { get; set; }
    public Guid TargetAccountId { get; set; }
    public int TransferTypeId { get; set; }
    public decimal Value { get; set; }
}
