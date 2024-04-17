using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TransactionAPI.Models;

namespace TransactionAPI.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser> 
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }

     
    }
}
