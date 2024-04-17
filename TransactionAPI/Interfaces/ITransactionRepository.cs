using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TransactionAPI.Data.Enums;
using TransactionAPI.Models;
using static TransactionAPI.Dto.ServiceResponses;

namespace TransactionAPI.Interfaces
{
    public interface ITransactionRepository
    {
        Task<bool> TransactionBelongsToUser(int transactionId, string userId);
        Task<List<Transaction>> GetTransactionsByUserId(string userId);

        Task<Transaction> GetTransactionByTransactionId(int transactionId);
        Task<Dictionary<Category, decimal>> GetSpendingBreakdownByPercent(string userId);
        Task<decimal> GetSumOfTransactions(string userId);
        Task<Transaction> GetMostExpensiveTransaction(string userId);
        Task<bool> CreateTransaction(Transaction transaction);
        Task<bool> UpdateTransaction(Transaction transaction);
        Task<bool> DeleteTransaction(Transaction transaction);
        Task<bool> Save();

    }
}

