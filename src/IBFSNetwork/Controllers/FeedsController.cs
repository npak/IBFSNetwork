using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBFSNetwork.Data;
using IBFSNetwork.Models.FeedModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

using IBFSNetwork.Models;

namespace IBFSNetwork.Controllers
{
    public class FeedsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public FeedsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Feeds
        public async Task<IActionResult> Index(int id)
        {
            if (User.IsInRole("admin"))
                return View(await _context.Feeds.ToListAsync());
            else
            {
                DataService.RssFeedService rss = new DataService.RssFeedService(_context);
                return View(rss.GetUserFeedList(_userManager.GetUserAsync(HttpContext.User).Result.Id));
            }
        }

        // GET: Feeds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feed = await _context.Feeds.SingleOrDefaultAsync(m => m.FeedId == id);
            if (feed == null)
            {
                return NotFound();
            }

            return View(feed);
        }

        // GET: Feeds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Feeds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FeedId,Desc,FeedType,IsDefault,Url")] Feed feed)
        {
            if (ModelState.IsValid)
            {
                feed.UserId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
                _context.Feeds.Add(feed);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(feed);
        }

        // GET: Feeds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feed = await _context.Feeds.SingleOrDefaultAsync(m => m.FeedId == id);
            if (feed == null)
            {
                return NotFound();
            }
            return View(feed);
        }
        // POST: Feeds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FeedId,Desc,FeedType,IsDefault,Url,UserId")] Feed feed)
        {
            if (id != feed.FeedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feed);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedExists(feed.FeedId))
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
            return View(feed);
        }

        // GET: Feeds/Edit/5
        public async Task<IActionResult> EditDefault(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feed = await _context.Feeds.SingleOrDefaultAsync(m => m.FeedId == id);
            if (feed == null)
            {
                return NotFound();
            }
            return View(feed);
        }
        // POST: Feeds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDefault(int id, [Bind("FeedId,Desc,FeedType,IsDefault,Url")] Feed feed)
        {
            if (id != feed.FeedId)
            {
                return NotFound();
            }

                try
                {
                UserFeed uf = new UserFeed();
                uf.FeedId = feed.FeedId;
                uf.UserId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
                    _context.UserFeeds.Add(uf);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {  
                        throw;
                }
                return RedirectToAction("Index");

            return View(feed);
        }

        // GET: Feeds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feed = await _context.Feeds.SingleOrDefaultAsync(m => m.FeedId == id);
            if (feed == null)
            {
                return NotFound();
            }

            return View(feed);
        }

        // POST: Feeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feed = await _context.Feeds.SingleOrDefaultAsync(m => m.FeedId == id);
            _context.Feeds.Remove(feed);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool FeedExists(int id)
        {
            return _context.Feeds.Any(e => e.FeedId == id);
        }
    }
}
