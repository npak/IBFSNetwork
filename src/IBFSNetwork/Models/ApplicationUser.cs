using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IBFSNetwork.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {

        public int? ClientId { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(258)]
        public string Location { get; set; }
        public bool IsSetUpEmailAlert { get; set; }
        public int? UserNumberCod { get; set; }

       // public virtual ICollection<AlertViewModels.Alert> Alert { get; set; }
    }

}
