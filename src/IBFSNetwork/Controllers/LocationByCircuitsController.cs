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

    public class LocationByCircuitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationByCircuitsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: LocationByCircuits
        public async Task<IActionResult> Index()
        {
            return View(await _context.LocationByCircuits.ToListAsync());
        }

        // GET: LocationByCircuits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationByCircuit = await _context.LocationByCircuits.SingleOrDefaultAsync(m => m.LocationByCircuitId == id);
            if (locationByCircuit == null)
            {
                return NotFound();
            }

            return View(locationByCircuit);
        }

        // GET: LocationByCircuits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LocationByCircuits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationByCircuitId,Caption")] LocationByCircuit locationByCircuit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(locationByCircuit);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(locationByCircuit);
        }

        // GET: LocationByCircuits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationByCircuit = await _context.LocationByCircuits.SingleOrDefaultAsync(m => m.LocationByCircuitId == id);
            if (locationByCircuit == null)
            {
                return NotFound();
            }
            return View(locationByCircuit);
        }

        // POST: LocationByCircuits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocationByCircuitId,Caption")] LocationByCircuit locationByCircuit)
        {
            if (id != locationByCircuit.LocationByCircuitId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(locationByCircuit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationByCircuitExists(locationByCircuit.LocationByCircuitId))
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
            return View(locationByCircuit);
        }

        // GET: LocationByCircuits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationByCircuit = await _context.LocationByCircuits.SingleOrDefaultAsync(m => m.LocationByCircuitId == id);
            if (locationByCircuit == null)
            {
                return NotFound();
            }

            return View(locationByCircuit);
        }

        // POST: LocationByCircuits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var locationByCircuit = await _context.LocationByCircuits.SingleOrDefaultAsync(m => m.LocationByCircuitId == id);
            _context.LocationByCircuits.Remove(locationByCircuit);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool LocationByCircuitExists(int id)
        {
            return _context.LocationByCircuits.Any(e => e.LocationByCircuitId == id);
        }
    }
}
