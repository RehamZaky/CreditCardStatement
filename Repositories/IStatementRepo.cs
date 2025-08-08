using CreditCardStatementApi.Data;
using CreditCardStatementApi.DTO;
using CreditCardStatementApi.Model;

namespace CreditCardStatementApi.Repositories
{
    public interface IStatementRepo
    {
        Task<List<Statement>> GetPeriodStatements(DatePeriodDTO periodDTO);

        Task<Statement> GetMonthStatement(string userId, DateOnly date);

        Task<Statement> AddStatement(Statement statement);

        Task<Transaction> AddTransaction(Transaction transaction);


    }
}
