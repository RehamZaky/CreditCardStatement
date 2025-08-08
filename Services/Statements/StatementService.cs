using AutoMapper;
using CreditCardStatementApi.DTO;
using CreditCardStatementApi.Model;
using CreditCardStatementApi.Repositories;

namespace CreditCardStatementApi.Services.Statements
{
    public class StatementService : IStatementService
    {
        private readonly IStatementRepo _statementRepo;
        private readonly IMapper _mapper;
        public StatementService(IStatementRepo statementRepo, IMapper mapper)
        {
            _statementRepo = statementRepo;
            _mapper = mapper;

        }
        public async Task<Statement> AddStatement(StatementDTO statementDTO)
        {

            var statement = _mapper.Map<StatementDTO, Statement>(statementDTO);
            statement.StatementMonth = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var availableStatement = _statementRepo.GetMonthStatement(statementDTO.UserId, statement.StatementMonth);
            if (availableStatement != null)
            {
                throw new InvalidOperationException("Statement of the month is already exist");
            }

            var statementResponse = await _statementRepo.AddStatement(statement);

            var transaction = _mapper.Map<TransactionDTO, Transaction>(statementDTO.TransactionsDTO);
            transaction.StatementId = statementResponse.Id;

            var transactionResponse = await _statementRepo.AddTransaction(transaction);
            return statementResponse;

        }

        public async Task<Statement> GetMonthStatement(DateDTO month)
        {
            var date = new DateOnly(month.Year, month.Month, 1);
            return await _statementRepo.GetMonthStatement(month.UserId, date);

        }

        public async Task<List<Statement>> GetPeriodStatements(DatePeriodDTO periodDTO)
        {
            return await _statementRepo.GetPeriodStatements(periodDTO);
        }
    }
}
