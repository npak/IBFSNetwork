using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBFSNetwork.Data;
using IBFSNetwork.Models.ForumViewModels;
using Microsoft.AspNetCore.Identity;
using IBFSNetwork.Models;

namespace IBFSNetwork.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationAdoContext _adocontext;

        public QuestionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ApplicationAdoContext adoContext)
        {
            _context = context;
            _userManager = userManager;
            _adocontext = adoContext;
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {
            var ApplicationDbContext = _context.Questions.Include(q => q.AppUser).Include(f=>f.Forum).Include(s=>s.SubThread);
            return View(await ApplicationDbContext.ToListAsync());
        }

        // GET: Questions
        public async Task<IActionResult> ViewForumList(int id)
        {
            DataService.ForumService forum = new DataService.ForumService(_context,_adocontext);
            return View("ViewForumList", forum.GetQuestionList(id));
        }

        // GET: Questions
        public async Task<IActionResult> Index2()
        {
            DataService.ForumService forum = new DataService.ForumService(_context,_adocontext);
            return View("View1", forum.GetLast10());
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.SingleOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Questions/Create
        public IActionResult Create(int id)
        {
            Question que = new Question();
            que.AppUserId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
            que.ForumId = id;
            return View(que);
        }

        // GET: Questions/Create
        public IActionResult SubCreate(int id)
        {
            Question que = new Question();
            que.AppUserId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
            que.SubThreadId = id;
            return View("Create",que);
        }
        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionId,AppUserId,DatePosted,ReplayCount,ForumId,SubThreadId,Title,ViewCount")] Question question)
        {
            if (ModelState.IsValid)
            {
                question.DatePosted = DateTime.Today;
                _context.Add(question);
                await _context.SaveChangesAsync();
                //forumsview/forum/2
                if (question.SubThreadId!=null)
                    return RedirectToAction("SubForum","ForumsView",new { id = question.SubThreadId });   
                else
                    return RedirectToAction("Forum", "ForumsView",new { id = question.ForumId });
            }
            //ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "AppUser", question.AppUserId);
            return View(question);
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.SingleOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionId,AppUserId,DatePosted,ReplayCount,Title,ViewCount")] Question question)
        {
            if (id != question.QuestionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.QuestionId))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "AppUser", question.AppUserId);
            return View(question);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.SingleOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var replies = _context.Replies.Where(i => i.QuestionId == id);
            foreach (var rs in replies)
                _context.Replies.Remove(rs);
            var question = await _context.Questions.SingleOrDefaultAsync(m => m.QuestionId == id);
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.QuestionId == id);
        }
    }
}
