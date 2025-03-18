namespace Shared.DTOs;

public record TransactionReadDto
{
    public int ID { get; set; }
    public Guid TransactionExternalId { get; set; }
    public Guid SourceAccountId { get; set; }
    public Guid TargetAccountId { get; set; }
    public int TransferTypeId { get; set; }
    public decimal Value { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
}

public record TransactionPostDto
{
    public Guid SourceAccountId { get; init; }
    public Guid TargetAccountId { get; init; }
    public int TransferTypeId { get; init; }
    public decimal Value { get; init; }
}
