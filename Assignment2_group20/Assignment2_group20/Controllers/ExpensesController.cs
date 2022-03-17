#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment2_group20.Data;
using Assignment2_group20.Hubs;
using Assignment2_group20.Models;
using Microsoft.AspNetCore.SignalR;

namespace Assignment2_group20.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly DataDb _context;
        private readonly IHubContext<ExpenseLogHub, IExpenseLogHub> _hubContext;

        public ExpensesController(DataDb context, IHubContext<ExpenseLogHub, IExpenseLogHub> hubContext)
        {
            _hubContext=hubContext;
            _context = context;
        }

        //GET: api/Expenses
       [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
        {
            return await _context.Expenses.ToListAsync();
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpense(long id)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            return expense;
        }
        

        // POST: api/Expenses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // Oprette en ny expense. Bemærk at en expense både er tilknyttet en model og et job.
        [HttpPost]
        public async Task<ActionResult<Expense>> PostExpense(Expense expense)
        {
            _context.Expenses.Add(expense);

            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.LogExpense(expense.ToString());

            return CreatedAtAction("GetExpense", new { id = expense.ExpenseId }, expense);
        }
        
        private bool ExpenseExists(long id)
        {
            return _context.Expenses.Any(e => e.ExpenseId == id);
        }
    }
}
