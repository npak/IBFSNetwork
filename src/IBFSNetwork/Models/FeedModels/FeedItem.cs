using IBFSNetwork.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBFSNetwork.Models
{
    public class FeedItem
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
    }
    ///

    /// <summary>
    /// Represents a feed item.
    /// </summary>
    public class FeedItemR
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string PublishDate { get; set; }
        public FeedType FeedType { get; set; }

        public FeedItemR()
        {
            Link = "";
            Title = "";
            Content = "";
            PublishDate = "";// DateTime.Today;
            FeedType = FeedType.RSS;
        }
    }

    public class FeedViewList
    {
        public List<FeedItemR> ListOfFeeds { get; set; }
    }

    public class FeedList
    {
//        public string Site { get; set; }
        public string Url { get; set; }
        public FeedType FeedType { get; set; }
    }

}
