
using IBFSNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using IBFSNetwork.DataService;
using IBFSNetwork.Data;
using Microsoft.AspNetCore.Identity;

namespace IBFSNetwork.Codes
{
    /// <summary>
    /// A simple RSS, RDF and ATOM feed parser.
    /// </summary>
    public class FeedParser
    {
        private readonly ApplicationDbContext _context;
        private  string _userId;
        private bool _isAdmin = false;
        public FeedParser(ApplicationDbContext context, string userId, bool isAdmin)
        {
            _context = context;
            _userId = userId;
            _isAdmin = isAdmin;
       }

        public IList<FeedItemR> FeedsURL { get; set; }

        /// <summary>
        /// Parses the given <see cref="FeedType"/> and returns a <see cref="IList&amp;lt;Item&amp;gt;"/>.
        /// </summary>
        /// <returns></returns>
        public Task<List<FeedItemR>> Parse(string url, FeedType feedType)
        {
            switch (feedType)
            {
                case FeedType.RSS:
                    return ParseRss(url);
                case FeedType.RDF:
                    return ParseRdf(url);
                case FeedType.Atom:
                    return ParseAtom(url);
                default:
                    throw new NotSupportedException(string.Format("{0} is not supported", feedType.ToString()));
            }
        }

        /// <summary>
        /// Parses an Atom feed and returns a <see cref="IList&amp;lt;Item&amp;gt;"/>.
        /// </summary>
        public async Task<List<FeedItemR>> ParseAtom(string url)
        {
            try
            {
                var client = new HttpClient();
                var stream = await client.GetStreamAsync(url);
                var doc = XDocument.Load(stream);

                //XDocument doc = XDocument.Load(url);
                // Feed/Entry
                var entries = from item in doc.Root.Elements().Where(i => i.Name.LocalName == "entry")
                              select new FeedItemR
                              {
                                  FeedType = FeedType.Atom,
                                  Content = item.Elements().First(i => i.Name.LocalName == "content").Value,
                                  Link = item.Elements().First(i => i.Name.LocalName == "link").Attribute("href").Value,
                                  PublishDate = ParseDate(item.Elements().First(i => i.Name.LocalName == "published").Value).ToString("yyyy-MM-dd"),
                                  Title = item.Elements().First(i => i.Name.LocalName == "title").Value
                              };
                return entries.ToList<FeedItemR>();
            }
            catch (Exception ex)
            {
                return new List<FeedItemR>();
            }
        }

        /// <summary>
        /// Parses an RSS feed and returns a <see cref="IList&amp;lt;Item&amp;gt;"/>.
        /// </summary>
        public async Task<List<FeedItemR>> ParseRss(string url)
        {
            try
            {
                var client = new HttpClient();
                var stream = await client.GetStreamAsync(url);
                var doc = XDocument.Load(stream);
                // RSS/Channel/item
                var entries = from item in doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().Where(i => i.Name.LocalName == "item")
                              select new FeedItemR
                              {
                                  FeedType = FeedType.RSS,
                                  Content = item.Elements().First(i => i.Name.LocalName == "description").Value,
                                  Link = item.Elements().First(i => i.Name.LocalName == "link").Value,
                                  PublishDate = ParseDate(item.Elements().First(i => i.Name.LocalName == "pubDate").Value).ToString("yyyy-MM-dd"),
                                  Title = item.Elements().First(i => i.Name.LocalName == "title").Value
                              };
                return entries.ToList<FeedItemR>();
            }
            catch (Exception ex)
            {
                return new List<FeedItemR>();
            }
        }

        public virtual IList<FeedItemR> ParseRssOld(string url)
        {
            try
            {
                XDocument doc = XDocument.Load(url);
                // RSS/Channel/item
                var entries = from item in doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().Where(i => i.Name.LocalName == "item")
                              select new FeedItemR
                              {
                                  FeedType = FeedType.RSS,
                                  Content = item.Elements().First(i => i.Name.LocalName == "description").Value,
                                  Link = item.Elements().First(i => i.Name.LocalName == "link").Value,
                                  PublishDate = ParseDate(item.Elements().First(i => i.Name.LocalName == "pubDate").Value).ToString("yyyy-MM-dd"),
                                  Title = item.Elements().First(i => i.Name.LocalName == "title").Value
                              };
                return entries.ToList();
            }
            catch (Exception ex)
            {
                return new List<FeedItemR>();
            }
        }

        /// <summary>
        /// Parses an RDF feed and returns a <see cref="IList&amp;lt;Item&amp;gt;"/>.
        /// </summary>
        public async Task<List<FeedItemR>> ParseRdf(string url)
        {
            try
            {
                var client = new HttpClient();
                var stream = await client.GetStreamAsync(url);
                var doc = XDocument.Load(stream);

               // XDocument doc = XDocument.Load(url);
                // <item> is under the root
                var entries = from item in doc.Root.Descendants().Where(i => i.Name.LocalName == "item")
                              select new FeedItemR
                              {
                                  FeedType = FeedType.RDF,
                                  Content = item.Elements().First(i => i.Name.LocalName == "description").Value,
                                  Link = item.Elements().First(i => i.Name.LocalName == "link").Value,
                                  PublishDate = ParseDate(item.Elements().First(i => i.Name.LocalName == "date").Value).ToString("yyyy-MM-dd"),
                                  Title = item.Elements().First(i => i.Name.LocalName == "title").Value
                              };
                return entries.ToList<FeedItemR>();
            }
            catch
            {
                return new List<FeedItemR>();
            }
        }

        private DateTime ParseDate(string date)
        {
            DateTime result;
            if (DateTime.TryParse(date, out result))
                return result;
            else
                return DateTime.MinValue;
        }


        //
        public async Task<List<FeedViewList>> GetFeeds()
        {
            List<FeedItemR> feed = new List<FeedItemR>();
            var feeds = new List<FeedViewList>();

            RssFeedService fs = new RssFeedService(_context);
            List<Models.FeedModels.Feed> feedList = new List<Models.FeedModels.Feed>();
            if (_isAdmin)
                feedList = fs.GetDefaultFeedList();
            else
            {
                feedList = fs.GetUserFeedList(_userId);
            }
            FeedViewList fview;
            foreach (var fl in feedList)
            {
                feed = await Parse(fl.Url, fl.FeedType);
                fview = new FeedViewList();
                fview.ListOfFeeds = feed;
                
                feeds.Add(fview);
            }
            return feeds;
        }

        //public List<FeedList> MyFeeds = new List<FeedList>()
        //{
        //    new FeedList { Url= "https://www.us-cert.gov/ncas/alerts.xml",FeedType=  FeedType.RSS },
        //    new FeedList {Url= "https://public.govdelivery.com/topics/USFDIC_117/feed.rss",FeedType=  FeedType.RSS },
        //    new FeedList { Url= "http://krebsonsecurity.com/feed/",FeedType=  FeedType.RSS },
        //    new FeedList { Url= "https://icc-ccs.org/component/obrss/1-icc-commercial-crime-services",FeedType=  FeedType.RSS },
        //    new FeedList {Url= "http://www.americanbanker.com/resources/americanbankerissues.xml",FeedType=  FeedType.RSS },


        //   new FeedList {Url= "http://acfeinsights.squarespace.com/?format=rss",FeedType=  FeedType.RSS },
        //   new FeedList { Url= "http://www.bankinfosecurity.com/rssFeeds.php?type=main",FeedType=  FeedType.RSS }




        //}; 



    }
    /// <summary>
    /// Represents the XML format of a feed.
    /// </summary>
    public enum FeedType
    {
        /// <summary>
        /// Really Simple Syndication format.
        /// </summary>
        RSS,
        /// <summary>
        /// RDF site summary format.
        /// </summary>
        RDF,
        /// <summary>
        /// Atom Syndication format.
        /// </summary>
        Atom
    }
}
