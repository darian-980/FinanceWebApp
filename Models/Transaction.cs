using System.ComponentModel.DataAnnotations.Schema;

namespace GroupProject.Models
{
    [Table("Transactiontbl")]
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public decimal AmountChange { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Now; // Set default value to current date and time

        public bool IsEdited { get; set; }


        // Navigation property
        public Account? Account { get; set; } 
    }

}
