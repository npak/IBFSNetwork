using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IBFSNetwork.Models.ManageViewModels
{
    public class ProfileModel
    {
        [Display(Name = "Setup Emai Alert")]
        public bool IsSetUpEmailAlert { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

    }
}
