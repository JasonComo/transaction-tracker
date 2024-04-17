using System.Diagnostics.Metrics;
using TransactionAPI.Data;
using TransactionAPI.Data.Enums;
using TransactionAPI.Models;


namespace TransactionAPI
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.Transactions.Any())
            {
                var transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Name = "Netflix Subscription",
                        DateTime = DateTime.Now,
                        Amount = 8,
                        Category = Category.Entertainment
                    }
                };
                dataContext.Transactions.AddRange(transactions);
                dataContext.SaveChanges();
            }
        }
    }
};
   
