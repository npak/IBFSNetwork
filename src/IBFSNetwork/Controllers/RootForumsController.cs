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

    public class RootForumsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RootForumsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: RootForums
        public async Task<IActionResult> Index()
        {
            return View(await _context.RootForums.ToListAsync());
        }

        // GET: RootForums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rootForum = await _context.RootForums.SingleOrDefaultAsync(m => m.RootForumId == id);
            if (rootForum == null)
            {
                return NotFound();
            }

            return View(rootForum);
        }

        // GET: RootForums/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RootForums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RootForumId,DateCreated,Title")] RootForum rootForum)
        {
            if (ModelState.IsValid)
            {
                rootForum.DateCreated = DateTime.Today;
                _context.Add(rootForum);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(rootForum);
        }

        // GET: RootForums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rootForum = await _context.RootForums.SingleOrDefaultAsync(m => m.RootForumId == id);
            if (rootForum == null)
            {
                return NotFound();
            }
            return View(rootForum);
        }

        // POST: RootForums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RootForumId,DateCreated,Title")] RootForum rootForum)
        {
            if (id != rootForum.RootForumId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rootForum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RootForumExists(rootForum.RootForumId))
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
            return View(rootForum);
        }

        // GET: RootForums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rootForum = await _context.RootForums.SingleOrDefaultAsync(m => m.RootForumId == id);
            ViewBag.ForumCount =  _context.Forums.Where(i => i.RootForumId == id).Count();
            if (rootForum == null)
            {
                return NotFound();
            }

            return View(rootForum);
        }

        // POST: RootForums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rootForum = await _context.RootForums.SingleOrDefaultAsync(m => m.RootForumId == id);
            _context.RootForums.Remove(rootForum);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool RootForumExists(int id)
        {
            return _context.RootForums.Any(e => e.RootForumId == id);
        }
    }
}
