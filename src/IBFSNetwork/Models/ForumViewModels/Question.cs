using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBFSNetwork.Models.ForumViewModels
{
    public class RootForum
    {
        [Key]
        public int RootForumId { get; set; }

        [Required]
        [Display(Name = "Title")]
        [MaxLength(511)]
        public string Title { get; set; }
        public int? LastMessageId { get; set; }
        public DateTime DateCreated { get; set; }

    }

    public class Forum
    {
        [Key]
        public int ForumId { get; set; }

        [Required]
        [Display(Name = "Title")]
        [MaxLength(511)]
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public int? LastMessageId { get; set; }
        public int? RootForumId { get; set; }
        public virtual RootForum RootForum { get; set; }
    }

    public class SubThread
    {
        [Key]
        public int SubThreadId { get; set; }

        [Required]
        [Display(Name = "Title")]
        [MaxLength(511)]
        public string Title { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        public int? LastMessageId { get; set; }

        public int ForumId { get; set; }
        public Forum Forum { get; set; }

    }

  
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        [Required]
        [Display(Name = "Title")]
        [MaxLength(511)]
        public string Title { get; set; }

        public DateTime DatePosted { get; set; }
        public int ReplyCount { get; set; }
        public int ViewCount { get; set; }

        [ForeignKey("AppUser")]
        public string AppUserId { get; set; } 
        public ApplicationUser AppUser { get; set; }

        public int? LastMessageId { get; set; }

        public int? SubThreadId { get; set; }
        public virtual SubThread SubThread { get; set; }

        public int? ForumId { get; set; }
        public virtual Forum Forum { get; set; }
        
    }

    public class QuestionViewItem
    {
        public int QuestionId { get; set; }

        public string Title { get; set; }
        public string UserId { get; set; }
        public DateTime DatePosted { get; set; }
        public int ReplyCount { get; set; }
        public int ViewCount { get; set; }
        
        public int? SubThreadId { get; set; }
        public int? ForumId { get; set; }

        public int QueId { get; set; }
        public string ByUser { get; set; }
        public string LastDate { get; set; }
    }

    public class Reply
    {
        [Key]
        public int ReplyId { get; set; }

        
        public int? QuestionId { get; set; }

        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }

        public DateTime ReplyDate { get; set; }

        [Required]
        [Display(Name = "Reply Message")]
        public string ReplyMsg { get; set; }

        public virtual Question Question { get; set; }
        public ApplicationUser AppUser { get; set; }
    }

    public class ViewReply
    {
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public int CommentsCount { get; set; }

        public int? ForumId { get; set; }
        public int? SubForumId { get; set; }

        public List<Reply> ReplyList { get; set; }
        public List<string> BreadCrumb { get; set; }

        //public string CurrentUserId { get; set; }
        //public string CurrentUserName { get; set; }

    }
}
