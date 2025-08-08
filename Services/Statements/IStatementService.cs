using CreditCardStatementApi.DTO;
using CreditCardStatementApi.Model;

namespace CreditCardStatementApi.Services.Statements
{
    public interface IStatementService
    {
        Task<List<Statement>> GetPeriodStatements(DatePeriodDTO periodDTO);

        Task<Statement> GetMonthStatement(DateDTO month);

        Task<Statement> AddStatement(StatementDTO statementDTO);
    }
}
