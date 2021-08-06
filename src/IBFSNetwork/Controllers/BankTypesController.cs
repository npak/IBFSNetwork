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

    public class BankTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BankTypesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: BankTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.BankTypes.OrderBy(c=>c.Caption).ToListAsync());
        }

        // GET: BankTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankType = await _context.BankTypes.SingleOrDefaultAsync(m => m.BankTypeId == id);
            if (bankType == null)
            {
                return NotFound();
            }

            return View(bankType);
        }

        // GET: BankTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BankTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BankTypeId,Caption")] BankType bankType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bankType);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(bankType);
        }

        // GET: BankTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankType = await _context.BankTypes.SingleOrDefaultAsync(m => m.BankTypeId == id);
            if (bankType == null)
            {
                return NotFound();
            }
            return View(bankType);
        }

        // POST: BankTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BankTypeId,Caption")] BankType bankType)
        {
            if (id != bankType.BankTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bankType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankTypeExists(bankType.BankTypeId))
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
            return View(bankType);
        }

        // GET: BankTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankType = await _context.BankTypes.SingleOrDefaultAsync(m => m.BankTypeId == id);
            if (bankType == null)
            {
                return NotFound();
            }

            return View(bankType);
        }

        // POST: BankTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bankType = await _context.BankTypes.SingleOrDefaultAsync(m => m.BankTypeId == id);
            _context.BankTypes.Remove(bankType);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool BankTypeExists(int id)
        {
            return _context.BankTypes.Any(e => e.BankTypeId == id);
        }
    }
}
