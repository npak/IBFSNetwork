using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace IBFSNetwork.Models.AlertViewModels
{
    public class AutocomleteFraudster
    {
        public int FraudsterId { get; set; }
        public string FullName { get; set; }

    }
    public class Fraudster
    {
        [Key]
        public int FraudsterId { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [MaxLength(50)]
        public string MiddleName { get; set; }

        [Display(Name = "DOB")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime BOD { get; set; }

        [Display(Name = "Gender")]
        [MaxLength(10)]
        public string Gender { get; set; }

        [Display(Name = "Company")]
        [MaxLength(128)]
        public string Company { get; set; }

        [Display(Name = "Address")]
        [MaxLength(512)]
        public string Address { get; set; }

        [Display(Name = "Alias")]
        [MaxLength(512)]
        public string Alias { get; set; }

        [Display(Name = "Phone")]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        [MaxLength(256)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //public bool isMain { get; set; }


        public virtual  List<FraudsterID> FraudsterIDs { get; set; }
        public virtual List<Document> Documents { get; set; }

        //public int AlertId { get; set; }
        //public Alert Alert { get; set; }

    }

    public class FraudsterID
    {
        [Key]
        public int PasportId { get; set; }
        
        [Required]
        [Display(Name = "ID Number")]
        [MaxLength(50)]
        public string PassportNumber { get; set; }

        [Display(Name = "Issuing State/Country")]
        [MaxLength(50)]
        public string IssuingCountry { get; set; }

        [Display(Name = "Issuing Authority")]
        [MaxLength(128)]
        public string IssuingAuthority { get; set; }

        [Display(Name = "Date of Issue")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DateOfIssue { get; set; }

        [Display(Name = "Expiration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ExpirationDate { get; set; }

        [Display(Name = "ID Type")]
        public int IDTypeId { get; set; }
        public IDType IDType { get; set; }

        [Required]
        public int FraudsterId { get; set; }

        public Fraudster frauster { get; set; }
    }


    public class IDType
    {
        public int IDTypeId { get; set; }

        [Required]
        [Display(Name = "Id Name")]
        [MaxLength(128)]
        public string IDTypeName { get; set; }
    }

    public class FraudsterWithID
    {
        public int FraudsterId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BOD { get; set; }
        public string Gender { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Alias { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string IDType { get; set; }
        public string PassportNumber { get; set; }
        public string IssuingCountry { get; set; }
        public string IssuingAuthority { get; set; }
        public DateTime? DateOfIssue { get; set; }
        public DateTime? ExpirationDate { get; set; }

    }

    public class FraudsterJson
    {
        public int FraudsterId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BOD { get; set; }
        public string Gender { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Alias { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public  List<IdsJonson> IDS { get; set; }
    }

    public class IdsJonson
    {
        public string IDType { get; set; }
        public string PassportNumber { get; set; }
        public string IssuingCountry { get; set; }
        public string IssuingAuthority { get; set; }
        public DateTime? DateOfIssue { get; set; }
        public DateTime? ExpirationDate { get; set; }
        
    }
    public class IdsJonsonWithId
    {
        public string IDType { get; set; }
        public string PassportNumber { get; set; }
        public string IssuingCountry { get; set; }
        public string IssuingAuthority { get; set; }
        public DateTime? DateOfIssue { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int FraudsterId { get; set; }
    }
    //public class AlertFraudser
    //{
    //    [Key]
    //    public int AlertFraudserId { get; set; }

    //    [Required]
    //    public int AlertId { get; set; }

    //    [Required]
    //    public int FraudsterId { get; set; }

    //    public Fraudster Fraudster { get; set; }
    //    public Alert Alert { get; set; }
    //}

}
