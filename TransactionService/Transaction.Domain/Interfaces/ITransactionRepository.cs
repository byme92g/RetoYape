using TransactionService.Transaction.Domain.Entities;

namespace TransactionService.Transaction.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<FinancialTransaction?> GetByExternalIdAsync(Guid transactionExternalId);
    Task AddAsync(FinancialTransaction transaction);
    Task UpdateAsync(FinancialTransaction transaction);
    Task<List<FinancialTransaction>> GetAll();
}
