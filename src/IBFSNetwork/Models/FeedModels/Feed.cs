using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IBFSNetwork.Models.FeedModels
{
    public class Feed
    {
        [Key]
        public int FeedId { get; set; }

        [Url]
        [Required]
        [Display(Name = "Url")]
        [MaxLength(255)]
       
        public String Url { get; set; }

        
        public Codes.FeedType FeedType { get; set; }

        [Display(Name = "Is Default")]
        public bool IsDefault { get; set; }

        [Display(Name = "Description")]
        [MaxLength(255)]
        public string Desc { get; set; }

        [MaxLength(450)]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

    }

    public class UserFeed
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(450)]
        [Required]
        public string  UserId { get; set; }
        [Required]
        public int FeedId { get; set; }

        public ApplicationUser User { get; set; }
        public Feed Feed { get; set; }

    }
}
