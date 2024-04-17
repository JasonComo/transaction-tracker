using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TransactionAPI.Data.Enums;

namespace TransactionAPI.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }

        public Category Category { get; set; }

        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }

}

