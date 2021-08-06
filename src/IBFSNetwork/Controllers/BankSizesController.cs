using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBFSNetwork.Data;
using IBFSNetwork.Models.AlertViewModels;
using Microsoft.AspNetCore.Authorization;

namespace IBFSNetwork.Controllers
{
    [Authorize(Roles = "admin")]
    public class BankSizesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BankSizesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: BankSizes
        public async Task<IActionResult> Index()
        {
            return View(await _context.BankSizes.OrderBy(c=>c.SortOrder).ToListAsync());
        }

        // GET: BankSizes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankSize = await _context.BankSizes.SingleOrDefaultAsync(m => m.BankSizeId == id);
            if (bankSize == null)
            {
                return NotFound();
            }

            return View(bankSize);
        }

        // GET: BankSizes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BankSizes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BankSizeId,Caption,SortOrder")] BankSize bankSize)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bankSize);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(bankSize);
        }

        // GET: BankSizes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankSize = await _context.BankSizes.SingleOrDefaultAsync(m => m.BankSizeId == id);
            if (bankSize == null)
            {
                return NotFound();
            }
            return View(bankSize);
        }

        // POST: BankSizes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BankSizeId,Caption,SortOrder")] BankSize bankSize)
        {
            if (id != bankSize.BankSizeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bankSize);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankSizeExists(bankSize.BankSizeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(bankSize);
        }

        // GET: BankSizes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankSize = await _context.BankSizes.SingleOrDefaultAsync(m => m.BankSizeId == id);
            if (bankSize == null)
            {
                return NotFound();
            }

            return View(bankSize);
        }

        // POST: BankSizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bankSize = await _context.BankSizes.SingleOrDefaultAsync(m => m.BankSizeId == id);
            _context.BankSizes.Remove(bankSize);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool BankSizeExists(int id)
        {
            return _context.BankSizes.Any(e => e.BankSizeId == id);
        }
    }
}
