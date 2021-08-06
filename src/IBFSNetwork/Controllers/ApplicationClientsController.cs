using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBFSNetwork.Data;
using IBFSNetwork.Models;
using Microsoft.AspNetCore.Authorization;

namespace IBFSNetwork.Controllers
{
    //[Area("Admin")]
    [Authorize(Roles = "admin")]

    public class ApplicationClientsController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        public ApplicationClientsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: ApplicationClients
        public async Task<IActionResult> Index()
        {
            return View(await _context.AplicationClients.ToListAsync());
        }

        // GET: ApplicationClients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationClients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,ClientCode,UserCount")] ApplicationClient applicationClient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationClient);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(applicationClient);
        }

        // GET: ApplicationClients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationClient = await _context.AplicationClients.SingleOrDefaultAsync(m => m.ClientId == id);
            if (applicationClient == null)
            {
                return NotFound();
            }
            return View(applicationClient);
        }

        // POST: ApplicationClients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientId,ClientCode,UserCount")] ApplicationClient applicationClient)
        {
            if (id != applicationClient.ClientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationClient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationClientExists(applicationClient.ClientId))
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
            return View(applicationClient);
        }

        // GET: ApplicationClients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationClient = await _context.AplicationClients.SingleOrDefaultAsync(m => m.ClientId == id);
            if (applicationClient == null)
            {
                return NotFound();
            }

            return View(applicationClient);
        }

        // POST: ApplicationClients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicationClient = await _context.AplicationClients.SingleOrDefaultAsync(m => m.ClientId == id);
            _context.AplicationClients.Remove(applicationClient);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ApplicationClientExists(int id)
        {
            return _context.AplicationClients.Any(e => e.ClientId == id);
        }
    }
}
