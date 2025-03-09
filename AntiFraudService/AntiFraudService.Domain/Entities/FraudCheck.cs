using Shared.Enums;

namespace AntiFraudService.AntiFraudService.Domain.Entities;

public class FraudCheck
{
    public Guid TransactionExternalId { get; set; }
    public decimal Value { get; set; }
    public FraudStatus Result { get; set; }
}
