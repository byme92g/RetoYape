using TransactionService.Transaction.Domain.Entities;
using TransactionService.Transaction.Domain.Interfaces;

namespace TransactionService.Transaction.Infraestructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionDbContext _context;

        public TransactionRepository(TransactionDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task AddAsync(FinancialTransaction transaction)
        {
            await _context.FinancialTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<FinancialTransaction?> GetByExternalIdAsync(Guid transactionExternalId)
        {
            return await _context.FinancialTransactions.FindAsync(transactionExternalId);
        }

        public async Task UpdateAsync(FinancialTransaction transaction)
        {
            _context.FinancialTransactions.Update(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
