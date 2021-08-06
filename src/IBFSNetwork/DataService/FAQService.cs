using IBFSNetwork.Data;
using IBFSNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IBFSNetwork.Models.FeedModels;
using System.Data.SqlClient;
using System.Data;
//using Microsoft.AspNetCore.Identity;
//using System.Security.Claims;


namespace IBFSNetwork.DataService
{
    public class FAQService
    {
        private readonly ApplicationDbContext _context;
        
        public FAQService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public List<Models.FeedModels.Feed> GetDefaultFeedList()
        {            
            return _context.Feeds.Where(g=>g.IsDefault).ToList();
        }

        public List<Models.FeedModels.Feed> GetUserFeedList(string id)
        {
                var query = from uf in _context.UserFeeds
                            join f in _context.Feeds on uf.FeedId equals f.FeedId
                            where uf.UserId == id
                            select f;
                return query.ToList();
        }


        public FaqResultWithPage SearchFaq(string qweStr, int page = 1, int pagesize = 20)
        {
            FaqResultWithPage faqs= new FaqResultWithPage();
                
            SqlConnection conn = new SqlConnection();
            try
            {  
                SqlDataReader rdr = null;
                Models.FaqModels.FAQ faq;

                conn.ConnectionString = _context.Database.GetDbConnection().ConnectionString;
                conn.Open();
                SqlCommand cmd = new SqlCommand("SearchFaqWithPage", conn);
                if (string.IsNullOrWhiteSpace(qweStr))
                { }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@qweStr", string.IsNullOrWhiteSpace(qweStr) ? "" : qweStr)); 
                cmd.Parameters.Add(new SqlParameter("@page", page));
                cmd.Parameters.Add(new SqlParameter("@pagesize", pagesize));
                rdr = cmd.ExecuteReader();
                int total = 0;

                while (rdr.Read())
                {
                    faq = new Models.FaqModels.FAQ();
                    total = Convert.ToInt32(rdr["TotalItems"]);
                    faq.FAQId = Convert.ToInt32(rdr["FAQId"]);
                    faq.Question = rdr["Question"].ToString();
                    faq.Replay = rdr["Replay"].ToString();
                    faq.UserId = rdr["UserId"].ToString();
                    faqs.Faqs.Add(faq);
                }
                rdr.Dispose();
                Models.PageInfo pageInfo = new Models.PageInfo(total, page, pagesize);
                faqs.PageInfo = pageInfo;

                return faqs;

            }
            catch (Exception ex)
            {
                return faqs;
            }
            finally
            { if (conn != null) conn.Close(); }
        }

    }
}
