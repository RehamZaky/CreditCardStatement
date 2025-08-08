using AutoMapper;
using CreditCardStatementApi.DTO;
using CreditCardStatementApi.Model;
using CreditCardStatementApi.Repositories;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace CreditCardStatementApi.Services.Statements
{
    public class StatementService : IStatementService
    {
        private readonly IStatementRepo _statementRepo;
        private readonly IMapper _mapper;
        private readonly IDatabase _redis;

        public StatementService(IStatementRepo statementRepo, IMapper mapper, IConnectionMultiplexer redis)
        {
            _statementRepo = statementRepo;
            _mapper = mapper;
            _redis = redis.GetDatabase();

        }
        public async Task<Statement> AddStatement(StatementDTO statementDTO)
        {

            if (statementDTO.AmountDue < 0 || statementDTO.TransactionsDTO.Amount < 0)
            {
                throw new Exception("Amounts can't be negative");
            }
            if (DateTime.UtcNow.CompareTo(statementDTO.TransactionsDTO.Date) < 0)
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
            var date = new DateOnly(month.Year, month.Month, 1);
            if(!ValidateDate(date))
            {
                throw new Exception("Date can't be in the future");
            }

            //check cache value
           var redisValue = await checkRedis(month.UserId);
            if(redisValue != null)
            {
                return _mapper.Map<StatementDTO>(redisValue);

            }

            // Get from DB
            var statement = await _statementRepo.GetMonthStatement(month.UserId, date);
            if (statement == null)
                return null;

            var statementDTO = _mapper.Map<StatementDTO>(statement);
            // Set Cache
            await SetRedis(month.UserId, statementDTO);

            return statementDTO;

        }


        public async Task<List<StatementDTO>> GetPeriodStatements(DatePeriodDTO periodDTO)
        {
            var date = new DateOnly(periodDTO.StartMonth.Year, periodDTO.StartMonth.Month, 1);
            if (!ValidateDate(date))
            {
                throw new Exception("Date can't be in the future");
            }

            var key = GetUserKey(periodDTO.UserId);
            var redisValue = await _redis.StringGetAsync(key);
            if (redisValue.HasValue)
            {
                return  JsonConvert.DeserializeObject<List<StatementDTO>>(redisValue);
            }



            var statement = await _statementRepo.GetPeriodStatements(periodDTO);

            if (statement.Count > 0)
            {
                var statementString = JsonConvert.SerializeObject(_mapper.Map<List<StatementDTO>>(statement));
                await _redis.StringSetAsync(key, statementString, TimeSpan.FromSeconds(30));
            }
            
            return _mapper.Map<List<StatementDTO>>(statement);


        }

        private async Task<StatementDTO> checkRedis(string userId)
        {
            var key = GetUserKey(userId);
            var redisValue = await _redis.StringGetAsync(key);
            if(redisValue.HasValue)
            {
               return JsonConvert.DeserializeObject<StatementDTO>(redisValue);
            }
            return null;
        }

        private async Task SetRedis(string userId,StatementDTO statement)
        {
            var key = GetUserKey(userId);
            var statementString = JsonConvert.SerializeObject(statement);
            await _redis.StringSetAsync(key, statementString,TimeSpan.FromSeconds(30));
        }

        private bool ValidateDate(DateOnly date)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (date.CompareTo(today) > 0)
            {
                return false;
            }
            return true;
         }
        private string GetUserKey(string userId) => $"statement:{userId}";
    }
}
