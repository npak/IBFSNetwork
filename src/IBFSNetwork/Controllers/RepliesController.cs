using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBFSNetwork.Data;
using IBFSNetwork.Models.ForumViewModels;
using IBFSNetwork.Models;
using Microsoft.AspNetCore.Identity;

namespace IBFSNetwork.Controllers
{
    public class RepliesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationAdoContext _adocontext;

        public RepliesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ApplicationAdoContext adoContext)
        {
            _context = context;
            _userManager = userManager;
            _adocontext = adoContext;
        }


        // GET: Replies
        public async Task<IActionResult> IndexOld(int id)
        {
            DataService.ForumService obj = new DataService.ForumService(_context,_adocontext);
            var que = obj.GetReplies(id);
            var cnt = que.Count();
            var bread = obj.GetReplyBreadcrumbTemp(id);
            // obj.QueViewCount(id);
            ViewBag.QuestionId = id;
            ViewBag.itle = obj.GeQuestionTitle(id);
            ViewBag.CurrentUserId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
            ViewBag.CurrentUserName = _userManager.GetUserAsync(HttpContext.User).Result.UserName;
            ViewBag.CommentsCount = cnt;
            ViewBag.BreadCrumb = bread;

            return View(que);

            //var applicationDbContext = _context.Replies.Where(i=>i.QuestionId==id).Include(r => r.AppUser).Include(r => r.Question);
            //return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Index(int id)
        {
            DataService.ForumService obj = new DataService.ForumService(_context,_adocontext);
            var que =  obj.GetRepliesNew(id);
            ViewBag.UserId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
            return View(que);
            
        }
        // GET: Replies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reply = await _context.Replies.SingleOrDefaultAsync(m => m.ReplyId == id);
            if (reply == null)
            {
                return NotFound();
            }

            return View(reply);
        }

        // GET: Replies/Create
        public IActionResult Create(int id)
        {
            Reply rep = new Reply();
            rep.QuestionId = id;
            rep.AppUserId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
            
            return View(rep);

        }

        // POST: Replies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReplyId,AppUserId,QuestionId,ReplyDate,ReplyMsg")] Reply reply)
        {

            if (ModelState.IsValid)
            {
                reply.ReplyDate = DateTime.Now;
                _context.Add(reply);
                await _context.SaveChangesAsync();
                DataService.ForumService forum = new DataService.ForumService(_context,_adocontext);
                forum.QueRepCount(reply.QuestionId,reply.ReplyId);
                return RedirectToAction("Index",new { id = reply.QuestionId });
            }
            
            return RedirectToAction("Index", new { id = reply.QuestionId });
        }

        [HttpPost]
        public async Task<string> CreateAjax()
        {
            Reply reply = new Reply();
            int queid = Convert.ToInt32(Request.Form["queid"].ToString());
            string userid = Request.Form["userid"].ToString();
            string text = Request.Form["text"].ToString();
            reply.ReplyDate = DateTime.Now;
            reply.QuestionId = queid;
            reply.AppUserId = userid;
            reply.ReplyMsg = text;
                _context.Add(reply);
                 _context.SaveChanges();
                DataService.ForumService forum = new DataService.ForumService(_context,_adocontext);
                forum.QueRepCount(reply.QuestionId,reply.ReplyId);
            string absUrl = Url.Action("Index", "Replies", new {id= queid }, Request.Scheme);
            return await Task.FromResult<string>(absUrl);        }

        [HttpPost]
        public async Task<string> EditAjax()
        {
            
            int queid = Convert.ToInt32(Request.Form["queid"].ToString());
            int replyid = Convert.ToInt32(Request.Form["replyid"].ToString());
            var reply =  _context.Replies.SingleOrDefault(m => m.ReplyId == replyid);
            string userid = Request.Form["userid"].ToString();
            string text = Request.Form["text"].ToString();

            reply.ReplyDate = DateTime.Now;
            //reply.QuestionId = queid;
            reply.AppUserId = userid;
            reply.ReplyMsg = text;
            _context.Update(reply);
            _context.SaveChanges();
            DataService.ForumService forum = new DataService.ForumService(_context,_adocontext);
            forum.DeleteQueRepCount(reply.QuestionId);
            string absUrl = Url.Action("Index", "Replies", new { id = queid }, Request.Scheme);
            return await Task.FromResult<string>(absUrl);
        }
        // GET: Replies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reply = await _context.Replies.SingleOrDefaultAsync(m => m.ReplyId == id);
            if (reply == null)
            {
                return NotFound();
            }
           
            return View(reply);
        }

        // POST: Replies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReplyId,AppUserId,QuestionId,ReplyDate,ReplyMsg")] Reply reply)
        {
            if (id != reply.ReplyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reply);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReplyExists(reply.ReplyId))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", reply.AppUserId);
            ViewData["QuestionId"] = new SelectList(_context.Questions, "QuestionId", "Title", reply.QuestionId);
            return View(reply);
        }

        // GET: Replies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reply = await _context.Replies.SingleOrDefaultAsync(m => m.ReplyId == id);
            if (reply == null)
            {
                return NotFound();
            }

            return View(reply);
        }

        // POST: Replies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reply = await _context.Replies.SingleOrDefaultAsync(m => m.ReplyId == id);
            _context.Replies.Remove(reply);
            await _context.SaveChangesAsync();
            DataService.ForumService forum = new DataService.ForumService(_context,_adocontext);
            forum.DeleteQueRepCount(reply.QuestionId,reply.ReplyId);

            return RedirectToAction("Index", new { id = reply.QuestionId } );
        }

        private bool ReplyExists(int id)
        {
            return _context.Replies.Any(e => e.ReplyId == id);
        }
    }
}
