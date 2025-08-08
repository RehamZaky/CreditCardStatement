using AutoMapper;
using CreditCardStatementApi.DTO;
using CreditCardStatementApi.Model;

namespace CreditCardStatementApi.Mappers
{
    public class StatementProfile : Profile
    {
        public StatementProfile() {
            CreateMap<Statement, StatementDTO>().ReverseMap();
            CreateMap<Transaction, TransactionDTO>().ReverseMap();  
        }
    }
}
