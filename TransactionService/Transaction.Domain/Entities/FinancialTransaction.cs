using SharedLib.Enums;
using System.ComponentModel.DataAnnotations;

namespace TransactionService.Transaction.Domain.Entities;

public class FinancialTransaction
{
    [Key]
    public int ID { get; set; }
    public FinancialTransaction()
    {
        TransactionExternalId = Guid.NewGuid();
        Value = 0m;
        CreatedAt = DateTime.UtcNow;
        Status = TransactionStatus.Pending;
    }

    public Guid TransactionExternalId { get; set; }

    [Required]
    public Guid SourceAccountId { get; set; }

    [Required]
    public Guid TargetAccountId { get; set; }

    [Required]
    public int TransferTypeId { get; set; }

    public decimal Value { get; set; }

    public DateTime CreatedAt { get; set; }

    public TransactionStatus Status { get; set; }
}
