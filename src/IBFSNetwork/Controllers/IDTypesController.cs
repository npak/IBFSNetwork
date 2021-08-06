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
    //[Area("Admin")]
    [Authorize(Roles = "admin")]

    public class IDTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IDTypesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: IDTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.IDTypes.ToListAsync());
        }

        // GET: IDTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iDType = await _context.IDTypes.SingleOrDefaultAsync(m => m.IDTypeId == id);
            if (iDType == null)
            {
                return NotFound();
            }

            return View(iDType);
        }

        // GET: IDTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IDTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDTypeId,IDTypeName")] IDType iDType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(iDType);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(iDType);
        }

        // GET: IDTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iDType = await _context.IDTypes.SingleOrDefaultAsync(m => m.IDTypeId == id);
            if (iDType == null)
            {
                return NotFound();
            }
            return View(iDType);
        }

        // POST: IDTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDTypeId,IDTypeName")] IDType iDType)
        {
            if (id != iDType.IDTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(iDType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IDTypeExists(iDType.IDTypeId))
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
            return View(iDType);
        }

        // GET: IDTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iDType = await _context.IDTypes.SingleOrDefaultAsync(m => m.IDTypeId == id);
            if (iDType == null)
            {
                return NotFound();
            }

            return View(iDType);
        }

        // POST: IDTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var iDType = await _context.IDTypes.SingleOrDefaultAsync(m => m.IDTypeId == id);
            _context.IDTypes.Remove(iDType);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool IDTypeExists(int id)
        {
            return _context.IDTypes.Any(e => e.IDTypeId == id);
        }
    }
}
