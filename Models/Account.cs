using System.ComponentModel.DataAnnotations.Schema;

namespace GroupProject.Models
{
    [Table("Accounttbl")]
    public class Account
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public bool Locked { get; set; }

        public User? User { get; set; }

    }
}
