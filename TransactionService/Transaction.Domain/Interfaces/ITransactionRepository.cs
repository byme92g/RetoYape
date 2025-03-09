namespace TransactionService.Transaction.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<Entities.Transaction> GetByExternalIdAsync(Guid transactionExternalId);
    Task AddAsync(Entities.Transaction transaction);
    Task UpdateAsync(Entities.Transaction transaction);
}
