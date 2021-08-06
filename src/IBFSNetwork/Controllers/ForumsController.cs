using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBFSNetwork.Data;
using IBFSNetwork.Models.ForumViewModels;

namespace IBFSNetwork.Controllers
{
    public class ForumsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ForumsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Forums
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Forums.Include(f => f.RootForum).OrderBy(f=>f.RootForum.Title).OrderBy(fr=>fr.Title);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Forums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forums.SingleOrDefaultAsync(m => m.ForumId == id);
            if (forum == null)
            {
                return NotFound();
            }

            return View(forum);
        }

        // GET: Forums/Create
        public IActionResult Create()
        {
            ViewData["RootForumId"] = new SelectList(_context.RootForums, "RootForumId", "Title");
            return View();
        }

        // POST: Forums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ForumId,DateCreated,RootForumId,Title")] Forum forum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(forum);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["RootForumId"] = new SelectList(_context.RootForums, "RootForumId", "Title", forum.RootForumId);
            return View(forum);
        }

        // GET: Forums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forums.SingleOrDefaultAsync(m => m.ForumId == id);
            if (forum == null)
            {
                return NotFound();
            }
            ViewData["RootForumId"] = new SelectList(_context.RootForums, "RootForumId", "Title", forum.RootForumId);
            return View(forum);
        }

        // POST: Forums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ForumId,DateCreated,RootForumId,Title")] Forum forum)
        {
            if (id != forum.ForumId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(forum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForumExists(forum.ForumId))
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
            ViewData["RootForumId"] = new SelectList(_context.RootForums, "RootForumId", "Title", forum.RootForumId);
            return View(forum);
        }

        // GET: Forums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forums.SingleOrDefaultAsync(m => m.ForumId == id);
            ViewBag.subForumCount = _context.SubThreads.Where(i => i.ForumId== id).Count();
            if (ViewBag.subForumCount==0)
                ViewBag.subForumCount = _context.Questions.Where(i => i.ForumId == id).Count();
            if (forum == null)
            {
                return NotFound();
            }

            return View(forum);
        }

        // POST: Forums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var forum = await _context.Forums.SingleOrDefaultAsync(m => m.ForumId == id);
            _context.Forums.Remove(forum);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ForumExists(int id)
        {
            return _context.Forums.Any(e => e.ForumId == id);
        }
    }
}
