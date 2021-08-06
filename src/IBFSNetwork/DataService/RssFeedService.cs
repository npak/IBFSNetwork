using IBFSNetwork.Data;
using IBFSNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IBFSNetwork.Models.FeedModels;
//using Microsoft.AspNetCore.Identity;
//using System.Security.Claims;


namespace IBFSNetwork.DataService
{
    public class RssFeedService
    {
        private readonly ApplicationDbContext _context;
        public RssFeedService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public List<Models.FeedModels.Feed> GetDefaultFeedList()
        {            
            return _context.Feeds.Where(g=>g.IsDefault).ToList();
        }

        public List<Models.FeedModels.Feed> GetUserFeedList(string id)
        {
            var query = (from f in _context.Feeds
                         where f.UserId == id
                         select f).Union(from fr in _context.Feeds
                                         where fr.IsDefault && !(from uf in _context.UserFeeds
                                                                 where uf.UserId == id
                                                                 select uf.FeedId).Contains(fr.FeedId)
                                         select fr);

                return query.ToList();
        }

        //public List<Models.FeedModels.Feed> GetUserFeedList(string id)
        //{
        //    var query = (from uf in _context.UserFeeds
        //                 join f in _context.Feeds on uf.FeedId equals f.FeedId
        //                 where uf.UserId == id
        //                 select f).Union((from def in _context.Feeds where def.IsDefault && !(from ) select def));
        //    return query.ToList();
        //}
    }
}
