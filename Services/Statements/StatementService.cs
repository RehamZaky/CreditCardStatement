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

            if (statementDTO.AmountDue < 0 || statementDTO.TransactionsDTO.Amount < 0)
            {
                throw new Exception("Amounts can't be negative");
            }
            if(DateTime.UtcNow.CompareTo(statementDTO.TransactionsDTO.Date) < 0)
            {
                throw new Exception("Transaction Date must be in the past");

            }

            var statement = _mapper.Map<StatementDTO, Statement>(statementDTO);
            statement.StatementMonth = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var availableStatement = await _statementRepo.GetMonthStatement(statementDTO.UserId, statement.StatementMonth);
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

        public async Task<StatementDTO> GetMonthStatement(DateDTO month)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var date = new DateOnly(month.Year, month.Month, 1);
            if(date.CompareTo(today) > 0)
            {
                throw new Exception("Date can't be in the future");
            }
            var statement = await _statementRepo.GetMonthStatement(month.UserId, date);
            if (statement == null)
                return null;
            return _mapper.Map<StatementDTO>(statement);

        }

        public async Task<List<StatementDTO>> GetPeriodStatements(DatePeriodDTO periodDTO)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var date = new DateOnly(periodDTO.StartMonth.Year, periodDTO.StartMonth.Month, 1);
            if (date.CompareTo(today) > 0)
            {
                throw new Exception("Date can't be in the future");
            }

            var statement = await _statementRepo.GetPeriodStatements(periodDTO);
            return _mapper.Map<List<StatementDTO>>(statement);
        }
    }
}
