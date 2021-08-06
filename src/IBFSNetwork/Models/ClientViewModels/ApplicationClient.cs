using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IBFSNetwork.Models
{
    public class ApplicationClient 
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        [Display(Name = "Client Code")]
        [MaxLength(10)]
        public string ClientCode { get; set; }

        [Required]
        [Display(Name = "User Count")]
        public int UserCount { get; set; }
      

    }

}
