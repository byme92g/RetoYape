using Microsoft.EntityFrameworkCore;
using TransactionService.Transaction.Domain.Entities;

namespace TransactionService.Transaction.Infraestructure;

public class TransactionDbContext : DbContext
{
    public TransactionDbContext(DbContextOptions options) : base(options) { }

    public DbSet<FinancialTransaction> FinancialTransactions { get; set; }
}
