using Shared.Enums;

namespace Shared.DTOs;

public record FraudCheckResponseDto(Guid TransactionExternalId, FraudStatus Result);
