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
    public class ForumsViewController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationAdoContext _adocontext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ForumsViewController(ApplicationDbContext context,ApplicationAdoContext adoContext, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _adocontext = adoContext;
            _userManager = userManager;
        }

        // GET: Root
        public IActionResult Index()
        {
            DataService.ForumService obj = new DataService.ForumService(_context,_adocontext);

            var root = obj.GetRootList();
            return View(root);
        }

        // GET: Root
        public IActionResult Forum(int id)
        {
            DataService.ForumService obj = new DataService.ForumService(_context,_adocontext);
            //var fourum = obj.GetForumModel(id);
            var fourum = obj.GetForumModelWithPage(id);
            fourum.UserId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
            return View(fourum);
        }

        // Post: Root
        [HttpPost]
        public IActionResult ReloadForumPage()
        {
            int forumid = Convert.ToInt32(Request.Form["forumid"].ToString());
            int page = Convert.ToInt32(Request.Form["page"].ToString());
            int pageSize = Convert.ToInt32(Request.Form["pagesize"].ToString());

            DataService.ForumService obj = new DataService.ForumService(_context,_adocontext);
            var viewmodel = obj.GetForumModelWithPage(forumid, page, pageSize);
            return PartialView("pForum", viewmodel);
        }


        // GET: Root
        public IActionResult SubForum(int id)
        {
            DataService.ForumService obj = new DataService.ForumService(_context,_adocontext);

            var subforum = obj.GetSubForumModel(id);
            subforum.UserId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
            return View(subforum);
        }

        // Post: Root
        [HttpPost]
        public IActionResult ReloadSubForumPage()
        {
            int subforumid = Convert.ToInt32(Request.Form["subforumid"].ToString());
            int page = Convert.ToInt32(Request.Form["page"].ToString());
            int pageSize = Convert.ToInt32(Request.Form["pagesize"].ToString());

            DataService.ForumService obj = new DataService.ForumService(_context,_adocontext);
            var viewmodel = obj.GetSubForumModel(subforumid, page, pageSize);
            return PartialView("pForum", viewmodel);
        }

    }
}
