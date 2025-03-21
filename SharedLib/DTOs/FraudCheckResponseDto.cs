using SharedLib.Enums;

namespace SharedLib.DTOs;

public record FraudCheckResponseDto(Guid TransactionExternalId, FraudStatus Result);
