using SharedLib.Enums;
using System.ComponentModel.DataAnnotations;

namespace TransactionService.Transaction.Domain.Entities;

public class FinancialTransaction
{
    public FinancialTransaction()
    {
        TransactionExternalId = Guid.NewGuid();
        Value = 0m;
        CreatedAt = DateTime.UtcNow;
        Status = TransactionStatus.Pending;
    }

    [Key]
    public int ID { get; set; }

    public Guid TransactionExternalId { get; set; }

    public Guid SourceAccountId { get; set; }

    public Guid TargetAccountId { get; set; }

    [EnumDataType(typeof(TransferType))]
    public TransferType TransferTypeId { get; set; }

    public decimal Value { get; set; }

    public DateTime CreatedAt { get; set; }

    [EnumDataType(typeof(TransactionStatus))]
    public TransactionStatus Status { get; set; }
}
