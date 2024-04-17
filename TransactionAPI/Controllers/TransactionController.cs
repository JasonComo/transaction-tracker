using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TransactionAPI.Data.Enums;
using TransactionAPI.Dto;
using TransactionAPI.Interfaces;
using TransactionAPI.Models;

namespace TransactionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionController(ITransactionRepository transactionRepository, UserManager<ApplicationUser> userManager)
        {
            _transactionRepository = transactionRepository;
            _userManager = userManager;
        }

        


        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetTransactionsByUserId()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("User is not authenticated.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is missing or invalid.");
            }

            try
            {
                var transactions = await _transactionRepository.GetTransactionsByUserId(userId);

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("spending-breakdown-percent")]
        [Authorize]
        public async Task<IActionResult> GetSpendingBreakdownByPercent()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("User is not authenticated.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is missing or invalid.");
            }

            try
            {
                var spendingBreakdown = await _transactionRepository.GetSpendingBreakdownByPercent(userId);
                return Ok(spendingBreakdown);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("transaction-sum")]
        [Authorize]
        public async Task<IActionResult> GetSumOfTransactions()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("User is not authenticated.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is missing or invalid.");
            }

            try
            {
                var transactions = await _transactionRepository.GetSumOfTransactions(userId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("most-expensive")]
        [Authorize]
        public async Task<IActionResult> GetMostExpensiveTransaction()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("User is not authenticated.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is missing or invalid.");
            }

            try
            {
                var mostExpensive = await _transactionRepository.GetMostExpensiveTransaction(userId);
                return Ok(mostExpensive);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionDTO transactionDTO)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("User is not authenticated.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is missing or invalid.");
            }

            var transaction = new Transaction
            {
                Name = transactionDTO.Name,
                DateTime = transactionDTO.DateTime,
                Amount = transactionDTO.Amount,
                Category = transactionDTO.Category,
                ApplicationUserId = userId
            };

            try
            {
                var success = await _transactionRepository.CreateTransaction(transaction);
                if (!success)
                {
                    return StatusCode(500, "Could not create transaction due to an internal error.");
                }
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("update/{transactionId}")]
        [Authorize]
        public async Task<IActionResult> UpdateTransaction(int transactionId, [FromBody] TransactionDTO transactionDTO)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("User is not authenticated.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (transactionId != transactionDTO.Id)
            {
                return BadRequest("Transaction ID mismatch.");
            }

            bool isOwner = await _transactionRepository.TransactionBelongsToUser(transactionId, userId);
            if (!isOwner)
            {
                return Forbid("You do not have permission to update this transaction.");
            }

            var existingTransactionTask = _transactionRepository.GetTransactionByTransactionId(transactionId);
            var existingTransaction = await existingTransactionTask;

            if (existingTransaction == null)
            {
                return NotFound("Transaction not found.");
            }

            existingTransaction.Name = transactionDTO.Name;
            existingTransaction.DateTime = transactionDTO.DateTime;
            existingTransaction.Amount = transactionDTO.Amount;
            existingTransaction.Category = transactionDTO.Category;

            try
            {
                var success = await _transactionRepository.UpdateTransaction(existingTransaction);
                if (!success)
                {
                    return StatusCode(500, "An error occurred while updating the transaction.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteTransaction([FromBody] int transactionId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("User is not authenticated.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool isOwner = await _transactionRepository.TransactionBelongsToUser(transactionId, userId);
            if (!isOwner)
            {
                return Forbid("you do not have permission to delete this transaction.");
            }

            var transactionTask = _transactionRepository.GetTransactionByTransactionId(transactionId);
            var transaction = await transactionTask;
            if (transaction == null)
            {
                return NotFound("Transaction not found.");
            }

            try
            {
                var success = await _transactionRepository.DeleteTransaction(transaction);
                if (!success)
                {
                    return StatusCode(500, "An error occurred while deleting the transaction.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}
