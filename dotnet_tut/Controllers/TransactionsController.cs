using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet_tut.Data;
using dotnet_tut.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace dotnet_tut.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TransactionsController(AppDbContext db) => _db = db;
  
        /// <summary>Gets all transaction, and filtering options are provided.</summary>
        /// <param name="transactionType">The transactionType payload.</param>
        /// <param name="minAmount">The minAmount payload.</param>
        /// <param name="maxAmount">The maxAmount payload.</param>
        /// <param name="fromDate">The fromDate payload.</param>
        /// <param name="toDate">The toDate payload.</param>
        /// <param name="customerName">The customerName payload.</param>
        /// <param name="customerId">The customerId payload.</param>
        /// <returns>The array of filtered transactions.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Transactions[]), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Transactions>>> GetTransactions(
            [FromQuery] TransactionTypeEnum? transactionType = null,
            [FromQuery] int? minAmount                      = null,
            [FromQuery] int? maxAmount                      = null,
            [FromQuery] DateTime? fromDate                  = null,
            [FromQuery] DateTime? toDate                    = null,
            [FromQuery] string customerName                 = null,
            [FromQuery] int? customerId                     = null
        )
        {
            var query = _db.Transactions
                           .Include(t => t.Customer)
                           .AsQueryable();

            if (transactionType.HasValue)
                query = query.Where(t => t.TransactionType == transactionType.Value);
            if (minAmount.HasValue)
                query = query.Where(t => t.Amount >= minAmount.Value);
            if (maxAmount.HasValue)
                query = query.Where(t => t.Amount <= maxAmount.Value);
            if (fromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(t => t.CreatedAt <= toDate.Value);
            if (!string.IsNullOrWhiteSpace(customerName))
                query = query.Where(t => t.Customer.FullName.Contains(customerName));
            if (customerId.HasValue)
                query = query.Where(t => t.CustomerId == customerId.Value);

            var list = await query.ToListAsync();
            return Ok(list);
        }



        /// <summary>Gets a transaction by id.</summary>
        /// <param name="id">The transaction payload.</param>
        /// <returns>The transaction found by the id param.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Transactions), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Transactions>> GetTransaction(int id)
        {
            var transaction = await _db.Transactions
                                       .Include(t => t.Customer)
                                       .FirstOrDefaultAsync(t => t.Id == id);
            if (transaction == null) return NotFound();
            return Ok(transaction);
        }



        /// <summary>Creates a new transaction.</summary>
        /// <param name="transaction">The transaction payload.</param>
        /// <returns>The created transaction with its new ID.</returns>
        [HttpPost("create")] 
        [ProducesResponseType(typeof(Transactions), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Transactions>> CreateTransaction(Transactions transaction)
        {
            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
        }


        /// <summary>Updates a transaction.</summary>
        /// <param name="id">The transaction payload.</param>
        /// <param name="updateTransaction">The transaction payload.</param>
        /// <returns>The updated transaction.</returns>
        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(Transactions), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async  Task<ActionResult<Transactions>> UpdateTransaction(
            [FromBody] Transactions updateTransaction,
            int id
        )
        {
            var transaction = await _db.Transactions.FindAsync(id);
            if (transaction == null)
                return NotFound($"No transaction found with ID = {id}");

        

            transaction.Amount               = updateTransaction.Amount;
            transaction.Description          = updateTransaction.Description;
            transaction.TransactionType      = updateTransaction.TransactionType;
            transaction.Status               = updateTransaction.Status;
        

            await _db.SaveChangesAsync();
            return Ok(transaction);
        }




        /// <summary>Deletes a transaction.</summary>
        /// <param name="id">The id payload.</param>
        /// <returns>No Content.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteTransaction(int id)
       {
            var transaction = await _db.Transactions.FindAsync(id);
            if (transaction == null) return NotFound();

            _db.Transactions.Remove(transaction);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>Gets a summary of  the transactions.</summary>
        /// <returns>The summary of the filtered transactions, or the summary of all transactions.</returns>
        [HttpGet("summary")]
        [ProducesResponseType(typeof(TransactionSummaryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TransactionSummaryDto>> GetTransactionSummary(
            [FromQuery] string customerName  = null,
            [FromQuery] string customerEmail = null,
            [FromQuery] string customerPhone = null
        )
        {
            var query = _db.Transactions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(customerName))
                query = query.Where(t => t.Customer.FullName.Contains(customerName));
            if (!string.IsNullOrWhiteSpace(customerEmail))
                query = query.Where(t => t.Customer.MainEmailAddress.Contains(customerEmail));
            if (!string.IsNullOrWhiteSpace(customerPhone))
                query = query.Where(t => t.Customer.MainPhoneNumber.Contains(customerPhone));

            var summary = await query
                .GroupBy(t => 1)  
                .Select(g => new TransactionSummaryDto
                {
                    TotalTransactions = g.Count(),
                    TotalCredits      = g.Where(t => t.TransactionType == TransactionTypeEnum.Credit)
                                          .Sum(t => t.Amount),
                    TotalDebits       = g.Where(t => t.TransactionType == TransactionTypeEnum.Debit)
                                          .Sum(t => t.Amount),
                    NetBalance        = g.Where(t => t.TransactionType == TransactionTypeEnum.Credit)
                                          .Sum(t => t.Amount)
                                       - g.Where(t => t.TransactionType == TransactionTypeEnum.Debit)
                                          .Sum(t => t.Amount)
                })
                .FirstOrDefaultAsync()
                ?? new TransactionSummaryDto();

            return Ok(summary);
        }
    }
}