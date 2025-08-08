using CreditCardStatementApi.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditCardStatementApi.DTO
{
    public class StatementDTO
    {
        public string UserId { get; set; }

        public DateTime DueDate { get; set; }

        public decimal AmountDue { get; set; }

        public TransactionDTO TransactionsDTO { get; set; }
    }

    public class TransactionDTO
    {
        public decimal Amount { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }
    }
}
