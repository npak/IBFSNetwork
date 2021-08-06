using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IBFSNetwork.Models;
using System.Net.Http;
using System.Xml.Linq;
using IBFSNetwork.Data;
using Microsoft.AspNetCore.Identity;

namespace IBFSNetwork.Controllers
{
    public class RSSFeedController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public RSSFeedController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager; ;
        }

        public async Task<IActionResult> Index()
        {
            List<FeedItemR> feed = new List<FeedItemR>();
            var feeds = new List<FeedViewList>();
         
            Codes.FeedParser fp = new Codes.FeedParser(_context, _userManager.GetUserAsync(HttpContext.User).Result.Id,User.IsInRole("admin"));

            return View("RssFeeds",await fp.GetFeeds());
        }

        public async Task<IActionResult> Index1()
        {
            var articles = new List< FeedItem>();
            //var feedUrl = "https://blogs.msdn.microsoft.com/martinkearn/feed/";
            var feedUrl = "http://krebsonsecurity.com/feed/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(feedUrl);
                var responseMessage = await client.GetAsync(feedUrl);
                var responseString = await responseMessage.Content.ReadAsStringAsync();

                //extract feed items
                XDocument doc = XDocument.Parse(responseString);
                var feedItems = from item in doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().Where(i => i.Name.LocalName == "item")
                                select new FeedItem
                                {
                                    Content = item.Elements().First(i => i.Name.LocalName == "description").Value,
                                    Link = item.Elements().First(i => i.Name.LocalName == "link").Value,
                                    PublishDate = ParseDate(item.Elements().First(i => i.Name.LocalName == "pubDate").Value),
                                    Title = item.Elements().First(i => i.Name.LocalName == "title").Value
                                };
                articles = feedItems.ToList();
            }

            return View(articles);
        }

        private DateTime ParseDate(string date)
        {
            DateTime result;
            if (DateTime.TryParse(date, out result))
                return result;
            else
                return DateTime.MinValue;
        }
    }
}

