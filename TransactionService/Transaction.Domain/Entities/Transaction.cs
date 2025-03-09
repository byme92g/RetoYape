using Shared.Enums;

namespace TransactionService.Transaction.Domain.Entities;

public class Transaction
{
    public Transaction()
    {
        TransactionExternalId = Guid.NewGuid();
        Value = 0m;
        CreatedAt = DateTime.UtcNow;
        Status = TransactionStatus.Pending;
    }

    public Guid TransactionExternalId { get; set; }

    public Guid SourceAccountId { get; set; }

    public Guid TargetAccountId { get; set; }

    public int TransferTypeId { get; set; }

    public decimal Value { get; set; }

    public DateTime CreatedAt { get; set; }
    public TransactionStatus Status { get; set; }

}
