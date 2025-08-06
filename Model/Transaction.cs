using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CreditCardStatementApi.Model
{
    public class Transaction
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public int StatementId { get; set; }
        [ForeignKey(nameof(StatementId))]

        public Statement statement { get; set; }    

    }
}
