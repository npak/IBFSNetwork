using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBFSNetwork.Models.ForumViewModels
{
    public class RootViewModel
    {
        public int RootForumId { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public List<ForumViewItem> Forums { get; set; }
        public RootViewModel()
        {
            Forums = new List<ForumViewItem>(); 
        }
    }

    public class ForumViewModel
    {
        public int ForumId { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserId { get; set; }
        public List<QuestionViewItem> Question { get; set; }
        public string ParentForum { get; set; }
        public List<SubThreadItem> SubThread { get; set; }
        public ForumViewModel()
        {
            SubThread = new List<SubThreadItem>();
            Question = new List<QuestionViewItem>();
        }
        public PageInfo PageInfo { get; set; }
    }
    public class ForumCount
    {
        public int Id { get; set; }
        public int Count { get; set; }
    }


    public class ForumTemp
    {
        public int RootForumId { get; set; }
        public int ForumId { get; set; }
        public string Title { get; set; }
        public DateTime DT { get; set; }
        public int ? QueId { get; set; }
        public string UserName { get; set; }
    }

    public class ForumQuestionsTemp
    {
        public int ForumId { get; set; }
        public int QuestionId { get; set; }
        public string Title { get; set; }

        public int ViewCount { get; set; }
        public int ReplyCount { get; set; }
        public int? SubThreadId { get; set; }

        public int? QueId { get; set; }
        public DateTime DT { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }

    }

    public class SubThreadTemp
    {
        public int QuestionId { get; set; }
        public int ForumId { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public int ViewCount { get; set; }
        public int ReplyCount { get; set; }
        public int SubThreadId { get; set; }

        public int? QueId { get; set; }
        public DateTime DT { get; set; }
        public string UserName { get; set; }
    }
    

    public class ForumViewItem
    {
        public int ForumId { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }

        public int DiscussionsCnt { get; set; }
        public int RepliesCnt { get; set; }
        public int SubForumsCnt { get; set; }
        public int QueId { get; set; }
        public string ByUser { get; set; }
        public string LastDate { get; set; }


        public List<SubThread> SubThread { get; set; }
        public ForumViewItem()
        {
            SubThread = new List<SubThread>();
        }
    }

    public class SubThreadView
    {
       public int SubThreadId { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public int ForumId { get; set; }
        public string ForumTitle { get; set; }
        public string ParentForum { get; set; }
        public List<QuestionViewItem> Question { get; set; }
        public int QueId { get; set; }
        public string ByUser { get; set; }
        public string LastDate { get; set; }

        public SubThreadView()
        {
            Question = new List<QuestionViewItem>();
        }
        public PageInfo PageInfo { get; set; }
    }
    public class SubThreadItem
    {
        public int SubThreadId { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }

        public int DiscussionsCnt { get; set; }
        public int RepliesCnt { get; set; }
        public int QueId { get; set; }
        public string ByUser { get; set; }
        public string LastDate { get; set; }
    }

   
}
