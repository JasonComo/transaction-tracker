using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransactionAPI.Data;
using TransactionAPI.Data.Enums;
using TransactionAPI.Interfaces;
using TransactionAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionAPI.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public TransactionRepository(UserManager<ApplicationUser> userManager, DataContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public async Task<bool> TransactionBelongsToUser(int transactionId, string userId)
        {
            return await _context.Transactions.AnyAsync(t => t.Id == transactionId && t.ApplicationUserId == userId);
        }
        public async Task<List<Transaction>> GetTransactionsByUserId(string userId)
        {
            return await _context.Transactions
                                 .Where(t => t.ApplicationUserId == userId)
                                 .ToListAsync();
        }

        public async Task<Transaction> GetTransactionByTransactionId(int transactionId)
        {
            return await _context.Transactions.Where(t => t.Id == transactionId).FirstOrDefaultAsync();
        }

        public async Task<Dictionary<Category, decimal>> GetSpendingBreakdownByPercent(string userId)
        {
            var transactions = await _context.Transactions
                                              .Where(t => t.ApplicationUserId == userId)
                                              .ToListAsync();
            decimal totalSpending = transactions.Sum(t => t.Amount);

            var breakdownByCategory = new Dictionary<Category, decimal>();

            // Iterate over each category and calculate its total spending and percentage of the total spending.
            foreach (var category in Enum.GetValues(typeof(Category)).Cast<Category>())
            {
                decimal categoryTotal = transactions.Where(t => t.Category == category).Sum(t => t.Amount);
                decimal categoryPercentage = totalSpending == 0 ? 0 : (categoryTotal / totalSpending) * 100;
                categoryPercentage = Math.Round(categoryPercentage, 1);

                // Exclude categories with no spending.
                if (categoryTotal > 0)
                {
                    breakdownByCategory.Add(category, categoryPercentage);
                }
            }

            breakdownByCategory = breakdownByCategory.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return breakdownByCategory;
        }

        public async Task<decimal> GetSumOfTransactions(string userId)
        {
            var transactions = await _context.Transactions
                                      .Where(t => t.ApplicationUserId == userId)
                                      .ToListAsync();

            decimal sumOfTransactions = transactions.Sum(t => t.Amount);

            return sumOfTransactions;
        }

        public async Task<Transaction> GetMostExpensiveTransaction(string userId)
        {
            return await _context.Transactions
                                 .Where(t => t.ApplicationUserId == userId)
                                 .OrderByDescending(t => t.Amount)
                                 .FirstOrDefaultAsync();
        }

        public async Task<bool> CreateTransaction(Transaction transaction)
        {
            try
            {
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateTransaction(Transaction transaction)
        {
            try
            {
                _context.Update(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTransaction(Transaction transaction)
        {
            try
            {
                _context.Remove(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }


    }
}
