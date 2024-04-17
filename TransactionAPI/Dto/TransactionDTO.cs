using TransactionAPI.Data.Enums;

namespace TransactionAPI.Dto
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
        public Category Category { get; set; }
    }
}
