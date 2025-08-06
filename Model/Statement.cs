using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditCardStatementApi.Model
{
    public class Statement
    {
        public int Id { get; set; }

        public string UserId { get; set; }   //GUID
        [ForeignKey(nameof(UserId))]

        public UserModel User { get; set; }

        public DateOnly StatementMonth { get; set; }  // "YYYY-MM"

        public DateTime DueDate { get; set; } = DateTime.UtcNow;

        public decimal AmountDue { get; set; }

        public List<Transaction> TransactionsList { get; set; }
    }
}
