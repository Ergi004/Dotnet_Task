using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet_tut.Data;
using dotnet_tut.Models;
using System;
using System.Linq;

namespace dotnet_tut.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TransactionsController(AppDbContext db) => _db = db;
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transactions>>> GetTransactions(
            [FromQuery] TransactionTypeEnum? transactionType,
            [FromQuery] int? minAmount,
            [FromQuery] int? maxAmount,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string customerName = null
        )
        {
            var query = _db.Transactions.AsQueryable();

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
                query = query.Where(t => t.CustomerFullName.Contains(customerName));


            var list = await query.ToListAsync();
            return Ok(list);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<Transactions>> GetTransaction(int id)
        {
            var transaction = await _db.Transactions.FindAsync(id);
            if (transaction == null) return NotFound();
            return transaction;
        }

        [HttpPost]
        public async Task<ActionResult<Transactions>> CreateTransaction(Transactions transaction)
        {
            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
        }


        [HttpPut("{id}")]
        public async  Task<ActionResult<Transactions>> UpdateTransaction(Transactions updateTransaction, int id)
        {
            var transaction = await _db.Transactions.FindAsync(id);
            if (transaction == null) return NotFound(new { Message = $"No user found with ID = {id}" });

            
            transaction.Amount = updateTransaction.Amount;
            transaction.CustomerFullName = updateTransaction.CustomerFullName;
            transaction.CustomerMainAddress = updateTransaction.CustomerMainAddress;
            transaction.CustomerMainPhoneNumber = updateTransaction.CustomerMainPhoneNumber;
            transaction.Description = updateTransaction.Description;
            transaction.TransactionType = updateTransaction.TransactionType;

            await _db.SaveChangesAsync();

            return updateTransaction ;
        }
        
         [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransaction(int id)
        {
            System.Diagnostics.Debug.WriteLine("This is a log");
            var transaction = await _db.Transactions.FindAsync(id);

            if (transaction == null) return NotFound();
            
            _db.Transactions.Remove(transaction);
            await _db.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("summary")]
        public async Task<ActionResult<TransactionSummaryDto>> GetTransactionSummary(
            [FromQuery] string customerName = null,
            [FromQuery] string customerPhone = null,
            [FromQuery] string customerEmail = null
        )
        {
            var query = _db.Transactions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(customerName))
                query = query.Where(t => t.CustomerFullName.Contains(customerName));

            if (!string.IsNullOrWhiteSpace(customerPhone))
                query = query.Where(t => t.CustomerMainPhoneNumber.Contains(customerPhone));

            if (!string.IsNullOrWhiteSpace(customerEmail))
                query = query.Where(t => t.CustomerMainEmailAddress.Contains(customerEmail));

            var summary = await query
                .GroupBy(t => 1)  
                .Select(g => new TransactionSummaryDto
                {
                    TotalTransactions = g.Count(),
                    TotalCredits = g
                        .Where(t => t.TransactionType == TransactionTypeEnum.Credit)
                        .Sum(t => t.Amount),
                    TotalDebits = g
                        .Where(t => t.TransactionType == TransactionTypeEnum.Debit)
                        .Sum(t => t.Amount),
                    NetBalance = g
                        .Where(t => t.TransactionType == TransactionTypeEnum.Credit)
                        .Sum(t => t.Amount) - g
                        .Where(t => t.TransactionType == TransactionTypeEnum.Debit)
                        .Sum(t => t.Amount)
                })
                .FirstOrDefaultAsync()
                ?? new TransactionSummaryDto();

            return Ok(summary);
        }
    }
}