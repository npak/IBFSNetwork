using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBFSNetwork.Data;
using IBFSNetwork.Models.ForumViewModels;
using Microsoft.AspNetCore.Authorization;

namespace IBFSNetwork.Controllers
{
    [Authorize(Roles = "admin")]

    public class SubThreadsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubThreadsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: SubThreads
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SubThreads.Include(s => s.Forum);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SubThreads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subThread = await _context.SubThreads.SingleOrDefaultAsync(m => m.SubThreadId == id);
            if (subThread == null)
            {
                return NotFound();
            }

            return View(subThread);
        }

        // GET: SubThreads/Create
        public IActionResult Create()
        {
            ViewData["ForumId"] = new SelectList(_context.Forums, "ForumId", "Title");
            return View();
        }

        // POST: SubThreads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubThreadId,DateCreated,ForumId,Title")] SubThread subThread)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subThread);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ForumId"] = new SelectList(_context.Forums, "ForumId", "Title", subThread.ForumId);
            return View(subThread);
        }

        // GET: SubThreads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subThread = await _context.SubThreads.SingleOrDefaultAsync(m => m.SubThreadId == id);
            if (subThread == null)
            {
                return NotFound();
            }
            ViewData["ForumId"] = new SelectList(_context.Forums, "ForumId", "Title", subThread.ForumId);
            return View(subThread);
        }

        // POST: SubThreads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubThreadId,DateCreated,ForumId,Title")] SubThread subThread)
        {
            if (id != subThread.SubThreadId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subThread);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubThreadExists(subThread.SubThreadId))
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
            ViewData["ForumId"] = new SelectList(_context.Forums, "ForumId", "Title", subThread.ForumId);
            return View(subThread);
        }

        // GET: SubThreads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subThread = await _context.SubThreads.SingleOrDefaultAsync(m => m.SubThreadId == id);

            if (subThread == null)
            {
                return NotFound();
            }
            ViewBag.DiscussionsCount = _context.Questions.Where(i => i.SubThreadId== id).Count();

            return View(subThread);
        }

        // POST: SubThreads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subThread = await _context.SubThreads.SingleOrDefaultAsync(m => m.SubThreadId == id);
            _context.SubThreads.Remove(subThread);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SubThreadExists(int id)
        {
            return _context.SubThreads.Any(e => e.SubThreadId == id);
        }
    }
}
