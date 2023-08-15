using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsDsII.api.Data;
using OsDsII.api.Models;


namespace OsDsII.api.Controllers.CustomerController
{
    [ApiController]
    [Route("[Controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly DataContext _context;

        public CustomerController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            List<Customer> listaCustomers = await _context.Customers.ToListAsync();
            return Ok(listaCustomers);
        }

        [HttpGet("{id}")] //BUSCAR POR ID
        public async Task<ActionResult<Customer>> GetCustomerById(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound(); // Retorna um resultado HTTP 404 NotFound se o cliente não for encontrado.
            }

            return customer;
        }

        [HttpPost] //CRIANDO CLIENTE
        public async Task<ActionResult> CreateCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
        }

        [HttpDelete("{id}")] //DELETAR UM CLIENTE POR ID 
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound(); // Retorna um resultado HTTP 404 NotFound se o cliente não for encontrado.
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna um resultado HTTP 204 NoContent após a exclusão bem-sucedida.
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomer(int id, Customer updatedCustomer)
        {
            if (id != updatedCustomer.Id)
            {
                return BadRequest(); // Retorna um resultado HTTP 400 BadRequest se o ID não corresponder.
            }

            _context.Entry(updatedCustomer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Customers.Any(c => c.Id == id))
                {
                    return NotFound(); // Retorna um resultado HTTP 404 NotFound se o cliente não for encontrado.
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Retorna um resultado HTTP 204 NoContent após a atualização bem-sucedida.
        }
    }
}


