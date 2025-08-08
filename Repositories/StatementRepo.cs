using CreditCardStatementApi.Data;
using CreditCardStatementApi.DTO;
using CreditCardStatementApi.Model;
using Microsoft.EntityFrameworkCore;

namespace CreditCardStatementApi.Repositories
{
    public class StatementRepo : IStatementRepo
    {
        private readonly ApplicationDBContext _context;

        public StatementRepo(ApplicationDBContext applicationDBContext)
        {
            _context = applicationDBContext;
        }
        public async Task<Statement> AddStatement(Statement statement)
        {
           await _context.statements.AddAsync(statement);
            await _context.SaveChangesAsync();
            return statement;
        }

        public async Task<Transaction> AddTransaction(Transaction transaction)
        {
            await _context.transactions.AddAsync(transaction);  
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Statement> GetMonthStatement(string userId, DateOnly date)
        {
           var statement = await _context.statements.Include(s=> s.TransactionsList).FirstOrDefaultAsync(s => s.UserId == userId &&
           s.StatementMonth.Year == date.Year &&
           s.StatementMonth.Month == date.Month);

            return statement;
        }

        public async Task<List<Statement>> GetPeriodStatements(DatePeriodDTO periodDTO)
        {
            return await _context.statements.Include(s=> s.TransactionsList)
                    .Where(s => s.StatementMonth.CompareTo(DateOnly.FromDateTime(periodDTO.StartMonth)) >= 0 &&
                                s.StatementMonth.CompareTo(DateOnly.FromDateTime(periodDTO.EndMonth)) <= 0)
                    .ToListAsync();
        }

    }
}
