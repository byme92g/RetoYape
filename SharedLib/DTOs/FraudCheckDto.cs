namespace SharedLib.DTOs;

public class FraudCheckDto
{
    public Guid TransactionExternalId { get; set; }
    public decimal Value { get; set; }
}
