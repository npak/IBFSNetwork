using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IBFSNetwork.Models.FaqModels
{
    public class FAQ
    {
        [Key]
        public int FAQId { get; set; }

        [Required]
        [Display(Name = "Question")]
        public string Question { get; set; }

        [Display(Name = "Answer")]
        public string Replay { get; set; }

        [Required]
        [MaxLength(450)]
        public string UserId { get; set; }

        public DateTime CreateDate { get; set; }

        public ApplicationUser User { get; set; }
    }
}
