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
    //  [Authorize(Policy = "AdminOnly")]
    // [Area("Admin")]
    [Authorize(Roles = "admin")]

    public class FraudTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FraudTypesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: FraudTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.FraudTypes.ToListAsync());
        }

        // GET: FraudTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fraudType = await _context.FraudTypes.SingleOrDefaultAsync(m => m.FraudTypeId == id);
            if (fraudType == null)
            {
                return NotFound();
            }

            return View(fraudType);
        }

        // GET: FraudTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FraudTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FraudTypeId,Caption")] FraudType fraudType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fraudType);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(fraudType);
        }

        // GET: FraudTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fraudType = await _context.FraudTypes.SingleOrDefaultAsync(m => m.FraudTypeId == id);
            if (fraudType == null)
            {
                return NotFound();
            }
            return View(fraudType);
        }

        // POST: FraudTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FraudTypeId,Caption")] FraudType fraudType)
        {
            if (id != fraudType.FraudTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fraudType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FraudTypeExists(fraudType.FraudTypeId))
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
            return View(fraudType);
        }

        // GET: FraudTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fraudType = await _context.FraudTypes.SingleOrDefaultAsync(m => m.FraudTypeId == id);
            if (fraudType == null)
            {
                return NotFound();
            }

            return View(fraudType);
        }

        // POST: FraudTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fraudType = await _context.FraudTypes.SingleOrDefaultAsync(m => m.FraudTypeId == id);
            _context.FraudTypes.Remove(fraudType);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool FraudTypeExists(int id)
        {
            return _context.FraudTypes.Any(e => e.FraudTypeId == id);
        }
    }
}
