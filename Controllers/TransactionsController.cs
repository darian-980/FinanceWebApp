using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupProject.Models;
using Microsoft.Identity.Client;

namespace GroupProject.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly UserContext _context;

        public TransactionsController(UserContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var transactions = await _context.Transaction
                .Include(t => t.Account)
                .ThenInclude(a => a.User) // Include the related User via Account
                .ToListAsync();

            return View(transactions);
        }



        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            // Retrieve Account and related User, and build SelectList
            var accountsWithUsers = _context.Account
                .Include(a => a.User)
                .Select(a => new
                {
                    AccountId = a.AccountId,
                    DisplayText = a.AccountName + " (" + a.User.Name + ")"
                }).ToList();

            ViewBag.Account = new SelectList(accountsWithUsers, "AccountId", "DisplayText");

            return View();
        }




        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionId,AccountId,AmountChange")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.TransactionDate = DateTime.Now;
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionId,AccountId,AmountChange")] Transaction transaction)
        {
            if (id != transaction.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    transaction.TransactionDate = DateTime.Now;
                    transaction.IsEdited = true;
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.TransactionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction != null)
            {
                _context.Transaction.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.TransactionId == id);
        }

        // GET: Transactions/Search
        public IActionResult Search()
        {
            return View();
        }

        // POST: Transactions/Search
        [HttpPost]
        public async Task<IActionResult> Search(int? accountId, decimal? amountChange, int? transactionId)
        {
            var transactions = _context.Transaction.AsQueryable();

            if (accountId.HasValue)
            {
                transactions = transactions.Where(t => t.AccountId == accountId);
            }

            if (amountChange.HasValue)
            {
                transactions = transactions.Where(t => t.AmountChange == amountChange);
            }

            if (transactionId.HasValue)
            {
                transactions = transactions.Where(t => t.TransactionId == transactionId);
            }

            return View("Index", await transactions.Include(t => t.Account).ToListAsync());
        }
    }
}
