using CreditCardStatementApi.DTO;
using CreditCardStatementApi.Model;

namespace CreditCardStatementApi.Services.Statements
{
    public interface IStatementService
    {
        Task<List<StatementDTO>> GetPeriodStatements(DatePeriodDTO periodDTO);

        Task<StatementDTO> GetMonthStatement(DateDTO month);

        Task<Statement> AddStatement(StatementDTO statementDTO);
    }
}
