using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBFSNetwork.Data;
using IBFSNetwork.Models.FaqModels;
using Microsoft.AspNetCore.Identity;
using IBFSNetwork.Models;

namespace IBFSNetwork.Controllers
{
    public class FAQsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FAQsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: FAQs
        public async Task<IActionResult> Index()
        {
            if (HttpContext.User.IsInRole("admin"))
                return View(await _context.FAQ.ToListAsync());

            return View(await _context.FAQ.Where(u=>u.UserId== _userManager.GetUserAsync(HttpContext.User).Result.Id).ToListAsync());
        }

        // GET: FAQs
        public async Task<IActionResult> SearchFaq(string searchstr, int page = 1, int pagesize = 20)
        {
            DataService.FAQService fs = new DataService.FAQService(_context);
            return View(fs.SearchFaq(searchstr, page, pagesize));
            //return View(await _context.FAQ.Where(u => u.UserId == _userManager.GetUserAsync(HttpContext.User).Result.Id).ToListAsync());

        }

        // GET: FAQs
        public IActionResult LoadSearchFaq()
        {
            int page = 1;
            page = Convert.ToInt32(Request.Form["page"].ToString());
            int pagesize = Convert.ToInt32(Request.Form["pagesize"].ToString());

            string searchstr = Request.Form["searchstr"].ToString();

            DataService.FAQService fs = new DataService.FAQService(_context);
            var faqs = fs.SearchFaq(searchstr, page, pagesize);
            return PartialView("_pFaq", faqs);
        }

        // GET: FAQs/Create
        public IActionResult Create()
        {
            FAQ faq = new FAQ();
            faq.UserId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
            return View(faq);
        }

        // POST: FAQs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FAQId,Question,Replaym,CreateDate,UserId")] FAQ fAQ)
        {
            fAQ.CreateDate = DateTime.Today;
            //fAQ.UserId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
            if (ModelState.IsValid)
            {   
                _context.Add(fAQ);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(fAQ);
        }

        // GET: FAQs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fAQ = await _context.FAQ.SingleOrDefaultAsync(m => m.FAQId == id);
            if (fAQ == null)
            {
                return NotFound();
            }
            return View(fAQ);
        }

        // POST: FAQs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FAQId,Question,Replay,CreateDate,UserId")] FAQ fAQ)
        {
            if (id != fAQ.FAQId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fAQ);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FAQExists(fAQ.FAQId))
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
            return View(fAQ);
        }

        // GET: FAQs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fAQ = await _context.FAQ.SingleOrDefaultAsync(m => m.FAQId == id);
            if (fAQ == null)
            {
                return NotFound();
            }

            return View(fAQ);
        }

        // POST: FAQs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fAQ = await _context.FAQ.SingleOrDefaultAsync(m => m.FAQId == id);
            _context.FAQ.Remove(fAQ);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool FAQExists(int id)
        {
            return _context.FAQ.Any(e => e.FAQId == id);
        }
    }
}
