using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_tut.Data;
using dotnet_tut.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_tut.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
     public class CustomerController : ControllerBase
     {
        private readonly AppDbContext _db;
        public CustomerController(AppDbContext db) => _db = db;


        /// <summary>Gets all customers, and filtering options are provided.</summary>
        /// <param name="customerName">The customerName payload.</param>
        /// <param name="customerEmail">The customerEmail payload.</param>
        /// <param name="customerPhoneNumber">The customerPhoneNumber payload.</param>
        /// <returns>The array of filtered customers.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Customer[]), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(
            [FromQuery] string customerName        = null,
            [FromQuery] string customerEmail       = null,
            [FromQuery] string customerPhoneNumber = null
        )
        {
            var query = _db.Customer
                 .Include(c => c.Transactions)
                 .AsQueryable();

            if (!string.IsNullOrWhiteSpace(customerName))
                query = query.Where(c => c.FullName.Contains(customerName));
            if (!string.IsNullOrWhiteSpace(customerEmail))
                query = query.Where(c => c.MainEmailAddress.Contains(customerEmail));
            if (!string.IsNullOrWhiteSpace(customerPhoneNumber))
                query = query.Where(c => c.MainPhoneNumber.Contains(customerPhoneNumber));

            var list = await query.ToListAsync();
            return Ok(list);
        }

        /// <summary>Gets a customer with the provided id.</summary>
        /// <param name="id">The id payload.</param>
        /// <returns>The customer and the transactions relating the customer.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _db.Customer
                                    .Include(c => c.Transactions)
                                    .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null) return NotFound();
            return Ok(customer);
        }


        /// <summary>Creates a new customer.</summary>
        /// <param name="customer">The customer payload.</param>
        /// <returns>The created customer.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Customer>> CreateCustomer([FromBody] Customer customer)
        {
            var exists = await _db.Customer
                                  .AnyAsync(c => c.MainEmailAddress == customer.MainEmailAddress);
            if (exists) return Conflict("This customer already exists!");

            _db.Customer.Add(customer);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }


        

        /// <summary>Updates a  customer.</summary>
        /// <param name="updateData">The updateData payload.</param>
        /// <param name="id">The id payload.</param>
        /// <returns>The updated customer.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
         public async Task<ActionResult<Customer>> UpdateCustomer(
            [FromBody] Customer updateData,
            int id
        )
        {
            var customer = await _db.Customer.FindAsync(id);
            if (customer == null) return NotFound("This customer does not exist!");

            customer.FullName         = updateData.FullName;
            customer.MainAddress      = updateData.MainAddress;
            customer.MainEmailAddress = updateData.MainEmailAddress;
            customer.MainPhoneNumber  = updateData.MainPhoneNumber;

            await _db.SaveChangesAsync();
            return Ok(customer);
        }


        /// <summary>Deletes a  customer.</summary>
        /// <param name="id">The id payload.</param>
        /// <returns>Status 200.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _db.Customer.FindAsync(id);
            if (customer == null) return NotFound("This customer does not exist!");

            _db.Customer.Remove(customer);
            await _db.SaveChangesAsync();
            return NoContent();
        }

     }
}