using IBFSNetwork.Data;
using IBFSNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IBFSNetwork.Models.ForumViewModels;
using System.Data.SqlClient;
using System.Data;

//using Microsoft.AspNetCore.Identity;
//using System.Security.Claims;


namespace IBFSNetwork.DataService
{
    public class ForumService
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationAdoContext _adocontext;

        //root forum
        private List<ForumCount> _subforumCountList;
        private List<ForumCount> _discussionsCounList;
        private List<ForumCount> _replyCountList;
        private List<ForumCount> _subDiscusCountList;
        private List<ForumCount> _subReplyCountList;

        // forums
        private List<ForumQuestionsTemp> _getForumQuestiFromSp;
        private List<SubThreadTemp> _getSubForumQuestionsSP;

        public ForumService(ApplicationDbContext context, ApplicationAdoContext adoContext)
        {
            _context = context;
            _adocontext = adoContext;
        }

        public List<Question> GetLast10()
        {
            return _context.Questions.OrderByDescending(g => g.DatePosted).Take(10).Include(a => a.AppUser).ToList();

        }
        public List<Question> GetQuestionList(int id)
        {
            return _context.Questions.Where(i => i.ForumId == id).OrderByDescending(g => g.DatePosted).Include(a => a.AppUser).ToList();
        }

        public List<Question> GetSubQuestionList(int id)
        {
            return _context.Questions.Where(i => i.SubThreadId == id).OrderByDescending(g => g.DatePosted).Include(a => a.AppUser).ToList();
        }
        public List<RootViewModel> GetRootList()
        {
            List<RootViewModel> res = new List<RootViewModel>();
            RootViewModel rv;
            var rootList = _context.RootForums;
            List<ForumViewItem> forum = new List<ForumViewItem>();

            //getforums
            var forumList = GetForumsFromSP();
            // get subforum count list by forum id
            SubforumCount();
            DiscussionsCount();
            ReplyCount();

            SubDiscusCount();
            SubReplyCount();

            try
            {
                foreach (var rf in rootList)
                {
                    rv = new RootViewModel();
                    rv.DateCreated = rf.DateCreated;
                    rv.Title = rf.Title;

                    rv.Forums = GetForums(rf.RootForumId, forumList);
                    res.Add(rv);
                }
            }
            catch(Exception ex)
            {
                throw ;
            }
            
            return res;
            //_context.RootForums.Include(a => a.).ToList();
        }

        //public ForumViewModel GetForumModel(int id)
        //{
        //    ForumViewModel fm = new ForumViewModel();
        //    //            var fm = _context.Forums.Where(i => i.ForumId == id).SingleOrDefault();
        //    fm = (from f1 in _context.Forums
        //          join p in _context.RootForums on f1.RootForumId equals p.RootForumId
        //          where f1.ForumId == id
        //          select new ForumViewModel
        //          {
        //              ForumId = f1.ForumId,
        //              Title = f1.Title,
        //              DateCreated = f1.DateCreated,
        //              ParentForum = p.Title
        //          }).SingleOrDefault();

        //    //).SingleOrDefault();

        //    var questions = _context.Questions.Where(q => q.ForumId == id);
        //    var sublist = _context.SubThreads.Where(s => s.ForumId == id);
        //    SubThreadItem item;
        //    List<SubThreadItem> resSub = new List<SubThreadItem>();
        //    foreach (var row in sublist)
        //    {
        //        item = new SubThreadItem();
        //        item.SubThreadId = row.SubThreadId;
        //        item.DateCreated = row.DateCreated;
        //        item.Title = row.Title;
        //        item.DiscussionsCnt = SubDiscusCount(row.SubThreadId);
        //        item.RepliesCnt = SubReplyCount(row.SubThreadId);
        //        resSub.Add(item);
        //    }
        //    if (fm != null)
        //    {

        //        //fmodel.ForumId = fm.ForumId;
        //        //fmodel.Title = fm.Title;
        //        //fmodel.DateCreated = fm.DateCreated;
        //        if (questions != null)
        //            fm.Question = questions.ToList();
        //        if (sublist != null)
        //            fm.SubThread = resSub;
        //    }
        //    return fm;
        //}

        public ForumViewModel GetForumModelWithPage(int forumid, int page=1, int pagesize=20)
        {
            ForumViewModel fm = new ForumViewModel();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = _adocontext.Database.GetDbConnection().ConnectionString;
            conn.Open();

            fm = (from f1 in _context.Forums
                  join p in _context.RootForums on f1.RootForumId equals p.RootForumId
                  where f1.ForumId == forumid
                  select new ForumViewModel
                  {
                      ForumId = f1.ForumId,
                      Title = f1.Title,
                      DateCreated = f1.DateCreated,
                      ParentForum = p.Title
                  }).SingleOrDefault();

            int questionsTotals = _context.Questions.Where(q => q.ForumId == forumid).Count();
            //
            GetForumQuestiFromSp(forumid, page, pagesize,conn);
            SubDiscusCount();
            SubReplyCount();
            try
            {
                var questions = GetForumQuestions();

                var sublist = _context.SubThreads.Where(s => s.ForumId == forumid);
                SubThreadItem item;
                List<SubThreadItem> resSub = new List<SubThreadItem>();
                foreach (var row in sublist)
                {
                    item = new SubThreadItem();
                    item.SubThreadId = row.SubThreadId;
                    item.DateCreated = row.DateCreated;
                    item.Title = row.Title;
                    item.DiscussionsCnt = GetCount(_subDiscusCountList,row.SubThreadId);
                    item.RepliesCnt = GetCount(_subReplyCountList,row.SubThreadId);
                    resSub.Add(item);
                }
                if (fm != null)
                {
                    if (questions != null)
                        fm.Question = questions.ToList();
                    if (sublist != null)
                        fm.SubThread = resSub;
                }
                PageInfo pi = new Models.PageInfo(questionsTotals, page, pagesize);
                fm.PageInfo = pi;
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            { if (conn != null) conn.Close(); }
            return fm;
        }

        


        public SubThreadView GetSubForumModel(int id, int page = 1, int pagesize = 20)
        {
            SubThreadView sub = new SubThreadView();
            int  questionsTotal = _context.Questions.Where(q => q.SubThreadId == id).Count();
            SqlConnection conn = new SqlConnection();
            try
            {
                conn.ConnectionString = _adocontext.Database.GetDbConnection().ConnectionString;
                conn.Open();
                GetSubForumQuestionsSP(id, page, pagesize,conn);
               var questions = GetSubForumQuestions();
                //_context.Questions.Where(q => q.SubThreadId == id).Skip((page-1)*pagesize).Take(pagesize);

                sub = (from th in _context.SubThreads
                       join fr in _context.Forums on th.ForumId equals fr.ForumId
                       join r in _context.RootForums on fr.RootForumId equals r.RootForumId
                       where th.SubThreadId == id
                       select new SubThreadView
                       {
                           SubThreadId = th.SubThreadId,
                           Title = th.Title,
                           ParentForum = r.Title,
                           ForumId = th.ForumId,
                           ForumTitle = fr.Title,
                           DateCreated = th.DateCreated,

                       }).SingleOrDefault();
                if (questions != null)
                    sub.Question = questions.ToList();
                PageInfo pi = new Models.PageInfo(questionsTotal, page, pagesize);
                sub.PageInfo = pi;
            }
           catch(Exception ex)
            {
                throw;
            }
            finally
            { if (conn != null) conn.Close(); }
            return sub;
        }

  

        public IEnumerable<Models.ForumViewModels.Reply> GetReplies(int qweId)
        {
            var que = (from q in _context.Questions
                       where q.QuestionId == qweId
                       select q).SingleOrDefault();
            if (que != null)
            {
                que.ViewCount = que.ViewCount + 1;

                try
                {
                    _context.Update(que);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }

            var query = _context.Replies.Where(r => r.QuestionId == qweId);
            return query;
            //var query = from uf in _context.UserFeeds
            //            join f in _context.Feeds on uf.FeedId equals f.FeedId
            //            where uf.UserId == id
            //            select f;
            //return query.ToList();
        }

        public ViewReply GetRepliesNew(int qweId)
        {
            ViewReply vr = new Models.ForumViewModels.ViewReply();
            var que = (from q in _context.Questions
                       where q.QuestionId == qweId
                       select q).SingleOrDefault();
            if (que != null)
            {
                que.ViewCount = que.ViewCount + 1;

                try
                {
                    _context.Update(que);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }
            var query = _context.Replies.Include(u => u.AppUser).Where(r => r.QuestionId == qweId);

            vr.CommentsCount = query.Count();

            if (que.ForumId != null)
                vr.ForumId = que.ForumId;
            if (que.SubThreadId != null)
            {
                vr.SubForumId = que.SubThreadId;
                var forum = _context.SubThreads.Where(s => s.SubThreadId == que.SubThreadId).SingleOrDefault();
                vr.ForumId = forum.ForumId;
            } 
            vr.Title = que.Title;
            vr.ReplyList = query.ToList();
            vr.BreadCrumb = GetReplyBreadcrumbByQid(qweId);
            vr.QuestionId = qweId;
            return vr;
            //var query = from uf in _context.UserFeeds
            //            join f in _context.Feeds on uf.FeedId equals f.FeedId
            //            where uf.UserId == id
            //            select f;
            //return query.ToList();
        }


        public string GeQuestionTitle(int qweId)
        {
            var tite = from q in _context.Questions
                       where q.QuestionId == qweId
                       select q;
            if (tite != null)
                return tite.SingleOrDefault().Title;
            else
                return "";
        }


        // breadCrumb
        public class BreadForum
        {
            public string RootTitle { get; set; }
            public string ForumTitle { get; set; }
        }

        public class BreadSubForum
        {
            public string RootTitle { get; set; }
            public string ForumTitle { get; set; }
            public string SubForumTitle { get; set; }

        }
        public  List<string> GetReplyBreadcrumbTemp(int qweId)
        {
            List<string> list = new List<string>();
            var qw = (from q in _context.Questions
                      where q.QuestionId == qweId
                      select q).SingleOrDefault();
            if (qw.ForumId != null)
            {
                var qwery = (from fr in _context.Forums
                             join r in _context.RootForums on fr.RootForumId equals r.RootForumId
                             where fr.ForumId == qw.ForumId
                             select new BreadForum
                             {
                                 RootTitle = r.Title,
                                 ForumTitle = fr.Title
                             }).SingleOrDefault();
                if (qwery != null)
                {
                    list.Add(qwery.RootTitle);
                    list.Add(qwery.ForumTitle);
                }
            }
            else
            {
                var qwery = (from su in _context.SubThreads
                             join fr in _context.Forums on su.ForumId equals fr.ForumId
                             join r in _context.RootForums on fr.RootForumId equals r.RootForumId
                             where fr.ForumId == qw.ForumId
                             select new BreadSubForum
                             {
                                 RootTitle = r.Title,
                                 ForumTitle = fr.Title,
                                 SubForumTitle = su.Title
                             }).SingleOrDefault();
                if (qwery != null)
                {
                    list.Add(qwery.RootTitle);
                    list.Add(qwery.SubForumTitle);
                    list.Add(qwery.ForumTitle);
                }
            }
            return list;
        }

        public  List<string> GetReplyBreadcrumbByQid(int qweId)
        {
            List<string> list = new List<string>();
            var qw = (from q in _context.Questions
                       where q.QuestionId == qweId
                       select q).SingleOrDefault();
            if (qw.ForumId != null)
            {
                var qwery = (from fr in _context.Forums
                            join r in _context.RootForums on fr.RootForumId equals r.RootForumId
                            where fr.ForumId == qw.ForumId
                            select new BreadForum
                            {
                                RootTitle= r.Title,
                                ForumTitle= fr.Title
                            }).SingleOrDefault() ;
                if (qwery != null)
                {
                    list.Add(qwery.RootTitle);
                    list.Add(qwery.ForumTitle);
                }
            }
            else
            {
                var qwery = (from su in _context.SubThreads
                             join fr in _context.Forums on su.ForumId equals fr.ForumId
                             join r in _context.RootForums on fr.RootForumId equals r.RootForumId
                             where su.SubThreadId == qw.SubThreadId
                             select new BreadSubForum
                             {
                                 RootTitle = r.Title,
                                 ForumTitle = fr.Title,
                                  SubForumTitle = su.Title
                             }).SingleOrDefault();
                if (qwery != null)
                {
                    list.Add(qwery.RootTitle);
                    list.Add(qwery.ForumTitle);
                    list.Add(qwery.SubForumTitle);
                }
            }
                return list;
        }

        //

        public void  QueViewCount(int queId)
        {
            var que = (from q in _context.Questions
                       where q.QuestionId == queId
                       select q).SingleOrDefault();
            if (que != null)
            {
                que.ViewCount = que.ViewCount + 1;

                try
                {
                    _context.Update(que);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!QuestionExists(question.QuestionId))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
            }
        }

        public async void QueRepCount(int? queId, int messageId)
        {
            var que = (from q in _context.Questions
                       where q.QuestionId == queId
                       select q).SingleOrDefault();
            if (que != null)
            {
                que.ReplyCount = que.ReplyCount + 1;
                que.LastMessageId = messageId;
                try
                {
                    _context.Update(que);
                    int rootid=0;
                    if (que.SubThreadId != null)
                    {
                        var sub = (from s in _context.SubThreads
                                   where s.SubThreadId == que.SubThreadId
                                   select s).SingleOrDefault();
                        if (sub != null)
                            sub.LastMessageId = messageId;

                        var forum = (from f in _context.Forums
                                     where f.ForumId == sub.ForumId
                                     select f).SingleOrDefault();
                        if (forum != null)
                            forum.LastMessageId = messageId;
                        rootid = forum.RootForumId.Value;
                    }
                    else if (que.ForumId!=null)
                    {
                        var forum = (from f in _context.Forums
                                   where f.ForumId == que.ForumId
                                   select f).SingleOrDefault();
                        if (forum != null)
                            forum.LastMessageId = messageId;
                        rootid = forum.RootForumId.Value;
                    }

                    var root = (from r in _context.RootForums
                                 where r.RootForumId == rootid
                                 select r).SingleOrDefault();
                    if (root != null)
                        root.LastMessageId = messageId;


                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!QuestionExists(question.QuestionId))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }


            }
        }


        public async void DeleteQueRepCount(int? queId, int messageId)
        {
            //udate relies count
            var que = (from q in _context.Questions
                       where q.QuestionId == queId
                       select q).SingleOrDefault();
            if (que != null)
            {
                if (que.ReplyCount > 0)
                {
                    try
                    {
                        que.ReplyCount = que.ReplyCount - 1;

                        //get last message id
                        int newLasMessageId = 0;
                        var qwe = (from r in _context.Replies
                                   where r.QuestionId == que.QuestionId
                                   orderby r.ReplyDate descending
                                   select r).Take(1).SingleOrDefault();

                        if (qwe != null)
                        {
                            // update questions
                            newLasMessageId = qwe.ReplyId;
                            que.LastMessageId = newLasMessageId;
                            // update subThread/Forum
                            if (que.SubThreadId != null)
                            {
                                var sub = (from s in _context.SubThreads
                                           where s.LastMessageId == messageId
                                           select s).Take(1).SingleOrDefault();
                                if (sub != null)
                                {
                                    sub.LastMessageId = newLasMessageId;
                                    _context.Update(sub);
                                }
                            }
                            else
                            {
                                var f = (from fr in _context.Forums
                                         where fr.LastMessageId == messageId
                                         select fr).SingleOrDefault();
                                if (f != null)
                                {
                                    f.LastMessageId = newLasMessageId;
                                    _context.Update(f);
                                }
                            }

                        }
                        _context.Update(que);
                        _context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                    }
                }
            }
           
        }

        public void DeleteQueRepCount(int? queId)
        {
            var que = (from q in _context.Questions
                       where q.QuestionId == queId
                       select q).SingleOrDefault();
            if (que != null)
            {
                if (que.ReplyCount > 0)
                {
                    que.ReplyCount = que.ReplyCount - 1;

                    try
                    {
                        _context.Update(que);
                       // _context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                    }
                }
            }
        }


        //var query = from person in people
        //            join pet in pets on person equals pet.Owner into gj
        //            from subpet in gj.DefaultIfEmpty()
        //            select new { person.FirstName, PetName = (subpet == null ? String.Empty : subpet.Name) };

        private List<ForumTemp> GetForumsFromSP()
        {
            List<ForumTemp> list = new List<ForumTemp>();
            SqlDataReader rdr = null;
            ForumTemp forum;
            SqlConnection conn = new SqlConnection();

            try
            {
                conn.ConnectionString = _adocontext.Database.GetDbConnection().ConnectionString;
                conn.Open();

                SqlCommand cmd = new SqlCommand("[GetForumsByRootId]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
               // cmd.Parameters.Add(new SqlParameter("@rootId", rootid));

                rdr = cmd.ExecuteReader();
                
                while (rdr.Read())
                {
                    forum = new ForumTemp();
                    forum.RootForumId = Convert.ToInt32(rdr["RootForumId"]);
                    forum.ForumId  = Convert.ToInt32(rdr["ForumId"]);
                    forum.Title = rdr["Title"].ToString();
                    
                    if (rdr["QuestionId"] != System.DBNull.Value)
                    {
                        forum.QueId = Convert.ToInt32(rdr["QuestionId"]);
                        forum.UserName = rdr["UserName"].ToString();
                        forum.DT = Convert.ToDateTime( rdr["ReplyDate"]);
                    }
                    list.Add(forum);
                }
                rdr.Dispose();
                cmd.Dispose();
                return list;
                
            }
            catch (Exception ex)
            {
                return list;
            }
            finally
            { if (conn != null) conn.Close(); }

        }

        public List<ForumViewItem> GetForums(int id,List<ForumTemp> forumlist)
        {
            List<ForumViewItem> list = new List<ForumViewItem>();
            List<ForumTemp> forums = forumlist.Where(u => u.RootForumId == id).ToList();
            List<SubThread> sub = _adocontext.SubThreads.ToList();
            if (forums.Count>0 )
            {
                ForumViewItem newrow;
                foreach (var row in forums)
                {
                    newrow = new ForumViewItem();
                    newrow.ForumId = row.ForumId;
                    newrow.Title = row.Title;
                    //newrow.DateCreated = row.DateCreated;
                    newrow.DiscussionsCnt = GetCount(_discussionsCounList,row.ForumId);
                    newrow.RepliesCnt = GetCount(_replyCountList,row.ForumId);
                    newrow.SubForumsCnt = GetCount(_subforumCountList,row.ForumId);
                    newrow.SubThread = sub.Where(u => u.ForumId == row.ForumId).ToList();
                    //_context.SubThreads.Where(i => i.ForumId == row.ForumId).ToList();
                    if (row.QueId != null)
                    {
                        newrow.QueId = row.QueId.Value;
                        newrow.LastDate = GetDate(row.DT);
                        newrow.ByUser = row.UserName;
                    }
                    list.Add(newrow);
                }
            }
            return list;
        }


         private string GetDate(DateTime d)
        {
            DateTime current = DateTime.Today;
            if (current.Year == d.Year && current.Month == d.Month && current.Day == d.Day)
                return "today:" + d.Hour.ToString()+ ":" +d.Minute.ToString();
            else if (current.Year == d.Year && current.Month == d.Month && (current.Day - d.Day)==1)
                return "yesterday:" + d.Hour.ToString() + ":" + d.Minute.ToString();
            else return d.ToString("MM/dd/yyyy") +"," +d.ToString("hh:mm");
        }

        private List<QuestionViewItem> GetForumQuestions()
        {
            List<QuestionViewItem> list = new List<QuestionViewItem>();

            
            //var que = GetForumQuestiFromSp(forumid, page, pagesize,conn);
            QuestionViewItem newrow;
            if (_getForumQuestiFromSp != null)
            {
                foreach (var row in _getForumQuestiFromSp)
                {
                    newrow = new QuestionViewItem();
                    newrow.QuestionId = row.QuestionId;
                    newrow.ForumId = row.ForumId;
                    newrow.Title = row.Title;
                    newrow.UserId = row.UserId;
                    newrow.SubThreadId = row.SubThreadId;
                    newrow.ReplyCount = row.ReplyCount;
                    newrow.ViewCount = row.ViewCount;
                    if (row.QueId != null)
                    {
                        newrow.LastDate = GetDate(row.DT);
                        newrow.QueId = row.QueId.Value;
                        newrow.ByUser = row.UserName;
                    }
                    list.Add(newrow);
                }
            }
            return list;
        }

        private void GetForumQuestiFromSp(int forumid, int page, int pagesize, SqlConnection conn)
        {
            List<ForumQuestionsTemp> list = new List<ForumQuestionsTemp>();
            //SqlConnection conn = new SqlConnection();

            try
            {
                SqlDataReader rdr = null;
                ForumQuestionsTemp fque;
               // conn.ConnectionString = _context.Database.GetDbConnection().ConnectionString;
                //conn.Open();
                SqlCommand cmd = new SqlCommand("GetForumsByForumId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@forumId", forumid));
                cmd.Parameters.Add(new SqlParameter("@page", page));
                cmd.Parameters.Add(new SqlParameter("@pagesize", pagesize));

                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    fque = new ForumQuestionsTemp();

                    fque.QuestionId = Convert.ToInt32(rdr["QuestionId"]);
                    fque.ForumId = Convert.ToInt32(rdr["ForumId"]);
                    fque.Title = rdr["Title"].ToString();
                    fque.ViewCount = Convert.ToInt32(rdr["ViewCount"]);
                    fque.ReplyCount = Convert.ToInt32(rdr["ReplyCount"]);
                    fque.UserId = rdr["AppUserId"].ToString();
                    if (rdr["SubThreadId"]!=DBNull.Value)
                        fque.SubThreadId = Convert.ToInt32(rdr["SubThreadId"]);
                    
                    if (rdr["QueId"] != System.DBNull.Value)
                    {
                        fque.QueId = Convert.ToInt32(rdr["QueId"]);
                        fque.UserName = rdr["UserName"].ToString();
                        fque.DT = Convert.ToDateTime(rdr["ReplyDate"]);
                    }
                    list.Add(fque);
                }
                _getForumQuestiFromSp = list;
                rdr.Dispose();
                

            }
            catch (Exception ex)
            {
                throw;   
            }
           
        }

        private List<QuestionViewItem> GetSubForumQuestions()
        {
            List<QuestionViewItem> list = new List<QuestionViewItem>();
            //var que = GetSubForumQuestionsSP(subforumid, page, pagesize,conn);

            QuestionViewItem newrow;
            if (_getSubForumQuestionsSP != null)
            {
                foreach (var row in _getSubForumQuestionsSP)
                {
                    newrow = new QuestionViewItem();
                    newrow.QuestionId = row.QuestionId;
                    newrow.ForumId = row.ForumId;
                    newrow.UserId = row.UserId;
                    newrow.Title = row.Title;
                    newrow.SubThreadId = row.SubThreadId;
                    newrow.ReplyCount = row.ReplyCount;
                    newrow.ViewCount = row.ViewCount;
                    if (row.QueId != null)
                    {
                        newrow.LastDate = GetDate(row.DT);
                        newrow.QueId = row.QueId.Value;
                        newrow.ByUser = row.UserName;
                    }
                    list.Add(newrow);
                }
            }
            return list;
        }

        private void GetSubForumQuestionsSP(int subforumid, int page, int pagesize, SqlConnection conn)
        {
            List<SubThreadTemp> list = new List<SubThreadTemp>();
            SqlDataReader rdr = null;
            //SqlConnection conn = new SqlConnection();

            try
            {
                SubThreadTemp fque;
               // conn.ConnectionString = _context.Database.GetDbConnection().ConnectionString;
                //conn.Open();
                SqlCommand cmd = new SqlCommand("GetSubForumsBysubForumId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@subforumId", subforumid));
                cmd.Parameters.Add(new SqlParameter("@page", page));
                cmd.Parameters.Add(new SqlParameter("@pagesize", pagesize));

                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    fque = new SubThreadTemp();

                    fque.QuestionId = Convert.ToInt32(rdr["QuestionId"]);
                    fque.UserId = rdr["AppUserId"].ToString();
                    fque.Title = rdr["Title"].ToString();
                    fque.ViewCount = Convert.ToInt32(rdr["ViewCount"]);
                    fque.ReplyCount = Convert.ToInt32(rdr["ReplyCount"]);
                    if (rdr["SubThreadId"] != DBNull.Value)
                        fque.SubThreadId = Convert.ToInt32(rdr["SubThreadId"]);

                    if (rdr["QueId"] != System.DBNull.Value)
                    {
                        fque.QueId = Convert.ToInt32(rdr["QueId"]);
                        fque.UserName = rdr["UserName"].ToString();
                        fque.DT = Convert.ToDateTime(rdr["ReplyDate"]);
                    }
                    list.Add(fque);
                }
                rdr.Dispose();
                _getSubForumQuestionsSP = list;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        private void SubforumCount()
        {
            var query = from q in _context.SubThreads
                      group q by q.ForumId into grpForumId
                      select new ForumCount
                      {
                          Id = grpForumId.Key,
                          Count = grpForumId.Count()
                      };
            _subforumCountList = query.ToList();

        }

        private void DiscussionsCount()
        {
            var query = from q in _context.Questions
                        where q.ForumId!=null
                      group q by q.ForumId into grpForumId
                      select new ForumCount
                      {
                          Id = grpForumId.Key.Value,
                          Count = grpForumId.Count()
                      };
           
            _discussionsCounList= query.ToList();

        }

        private void ReplyCount()
        {
            var query = from f in _context.Forums
                       join q in _context.Questions on f.ForumId equals q.ForumId
                       join r in _context.Replies on q.QuestionId equals r.QuestionId
                      group f by f.ForumId into grpForumId
                      select new ForumCount
                      {
                          Id = grpForumId.Key,
                          Count = grpForumId.Count()
                      };
            _replyCountList= query.ToList();

        }

        // suforum
        //private List<SubThreadItem> GetSubForums(int id)
        //{
        //    List<SubThreadItem> list = new List<SubThreadItem>();
        //    var subforums = _context.SubThreads.Where(i => i.ForumId == id);
        //    SubThreadItem newrow;
        //    foreach (var row in subforums)
        //    {
        //        newrow = new SubThreadItem();
        //        newrow.SubThreadId = row.SubThreadId;
        //        newrow.Title = row.Title;
        //        newrow.DateCreated = row.DateCreated;
        //        newrow.DiscussionsCnt =  DiscussionsCount(row.ForumId);
        //        newrow.RepliesCnt = ReplyCount(row.ForumId);
        //        list.Add(newrow);
        //    }
        //    return list;
        //}

        private void SubDiscusCount()
        {
            var query = from q in _context.Questions
                        where q.SubThreadId !=null
                         group q by q.SubThreadId into grpForumId
                         select new ForumCount
                         {
                             Id = grpForumId.Key.Value,
                             Count = grpForumId.Count()
                         };
            _subDiscusCountList= query.ToList();
        }
        private void SubReplyCount()
        {
            var query = from s in _context.SubThreads
                       join q in _context.Questions on s.SubThreadId equals q.SubThreadId
                       join r in _context.Replies on q.QuestionId equals r.QuestionId
                       group s by s.SubThreadId into grpForumId
                       select new ForumCount
                       {
                           Id = grpForumId.Key,
                           Count = grpForumId.Count()
                       };
            _subReplyCountList= query.ToList();
        }
        private int GetCount(List<ForumCount> list, int id)
        {
            var qwe = list.Where(i => i.Id == id).SingleOrDefault();

            if (qwe == null)
                return 0;
            else
                return qwe.Count;
        }


    }
}
