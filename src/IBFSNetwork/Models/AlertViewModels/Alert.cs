using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBFSNetwork.Models.AlertViewModels
{
    /// <summary>
    /// main alert model )
    /// </summary>
    public class Alert
    {
        [Key]
        public int AlertId { get; set; }

        [Required]
        [Display(Name = "Location")]
        public int LocationId { get; set; }

        [Display(Name = "Bank Size")]
        public int? BankSizeId { get; set; }

        [Display(Name = "Bank Type")]
        public int? BankTypeId { get; set; }

        [Display(Name = "Circuit Courts")]
        public int? LocationByCircuitId  { get; set; }

        [Display(Name = "State")]
        public int? LocationStateId { get; set; }

        [Display(Name = "City")]
        [MaxLength(50)]
        public string City { get; set; }

        [Display(Name = "Country")]
        
        public int? CountryId { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Required]
        [Display(Name = "Loss Amount")]
        public int LostAmount { get; set; }

        [Required]
        [Display(Name = "Loss Date")]
        [DataType(DataType.Date)]
        [DisplayFormat( DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime AlertDate { get; set; }


        [Required]
        [Display(Name = "Fraud Type")]
        public int FraudTypeId { get; set; }

        [MaxLength(450)]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public FraudType FraudType { get; set; }
        public virtual BankType BankType { get; set; }
        public virtual BankSize BankSize { get; set; }
        public Location Location { get; set; }
        public LocationByCircuit LocationByCircuit { get; set; }
        public LocationState LocationState { get; set; }
        public Country Country { get; set; }

        public virtual List<Fraudster> Fraudsters { get; set; }
        public AlertFraudster AlertFraudster { get; set; }
        public Alert()
        {
            Fraudsters = new List<Fraudster>();
            AlertFraudster = new AlertViewModels.AlertFraudster();
        }
    }

    public class AlertFraudster
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AlertId { get; set; }

        [Required]
        public int FraudsterId { get; set; }

        public bool isMain { get; set; }
        public Alert Alert { get; set; }
        public Fraudster Fraudster { get; set; }
    }
    //public class AlertOLD
    //{
    //    [Key]
    //    public int AlertId { get; set; }

    //    [Required]
    //    [Display(Name = "Location")]
    //    public int LocationId { get; set; }

    //    [Display(Name = "Bank Size")]
    //    public int? BankSizeId { get; set; }

    //    [Display(Name = "Bank Type")]
    //    public int? BankTypeId { get; set; }

    //    [Display(Name = "Circuit Courts")]
    //    public int? LocationByCircuitId { get; set; }

    //    [Display(Name = "State")]
    //    public int? LocationStateId { get; set; }

    //    [Display(Name = "City")]
    //    [MaxLength(50)]
    //    public string City { get; set; }

    //    [Display(Name = "Country")]

    //    public int? CountryId { get; set; }

    //    [Display(Name = "Notes")]
    //    public string Notes { get; set; }

    //    [Required]
    //    [Display(Name = "Loss Amount")]
    //    public int LostAmount { get; set; }

    //    [Required]
    //    [Display(Name = "Loss Date")]
    //    [DataType(DataType.Date)]
    //    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    //    public DateTime AlertDate { get; set; }


    //    [Required]
    //    [Display(Name = "Fraud Type")]
    //    public int FraudTypeId { get; set; }

    //    [MaxLength(450)]
    //    public string ApplicationUserId { get; set; }

    //    public virtual ApplicationUser ApplicationUser { get; set; }

    //    public FraudType FraudType { get; set; }
    //    public virtual BankType BankType { get; set; }
    //    public virtual BankSize BankSize { get; set; }
    //    public Location Location { get; set; }
    //    public LocationByCircuit LocationByCircuit { get; set; }
    //    public LocationState LocationState { get; set; }
    //    public Country Country { get; set; }

    //    public virtual List<Document> Documents { get; set; }
    //    public virtual List<Fraudster> Fraudsters { get; set; }
    //    public Alert()
    //    {
    //        Documents = new List<Document>();
    //        Fraudsters = new List<Fraudster>();
    //    }
    //}

    public class AlertForLanding
    {
        public int AlertId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Bank Size")]
        public string BankSize { get; set; }

        [Display(Name = "Bank Type")]
        public string BankType { get; set; }

        [Display(Name = "Circuit Courts")]
        public int? LocationByCircuitId { get; set; }

        [Display(Name = "State")]
        public string LocationState { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Loss Amount")]
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public int LostAmount { get; set; }

        [Display(Name = "Alert Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime AlertDate { get; set; }


        [Display(Name = "Fraud Type")]
        public string FraudType { get; set; }

        public string ApplicationUserId { get; set; }
    }


    /// <summary>
    /// Model for Landing Page
    /// </summary>

    public class AlertDisplayModel
    {
        
        public List<AlertForLanding> alerts { get; set; }
        public List<FeedViewList> feeds { get; set; }
        public IndustryLossChartModel IndustyLoss { get; set; }
        public IndustryLossChartModel IndustyTrend { get; set; }
        public List<Models.ForumViewModels.Question> Questions { get; set; }

    }

    public class FraudType
    {
        public int FraudTypeId { get; set; }

        [Required]
        [Display(Name = "Fraud Type")]
        [MaxLength(128)]
        public string Caption { get; set; }
    }

    public class BankType
    {
        public int BankTypeId { get; set; }

        [Required]
        [MaxLength(128)]
        public string Caption { get; set; }
    }

    public class BankSize
    {
        public int BankSizeId { get; set; }

        [Required]
        [MaxLength(128)]
        [Display(Name = "Bank SIze")]
        public string Caption { get; set; }

        [Display(Name = "Sort Order")]
        public int SortOrder { get; set; }
    }

    public class Location
    {
        public int LocationId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Caption { get; set; }
    }

    public class LocationByCircuit
    {
        public int LocationByCircuitId { get; set; }

        [Required]
        [Display(Name = "Circuit Courts")]
        [MaxLength(128)]
        public string Caption { get; set; }
    }

    public class LocationState 
    {
        public int LocationStateId { get; set; }


        [Display(Name = "Code")]
        [MaxLength(10)]
        public string Code { get; set; }

        [Required]
        [Display(Name = "State")]
        [MaxLength(128)]
        public string Caption { get; set; }
    }

    public class Country
    {
        public int CountryId { get; set; }

        [Display(Name = "Code")]
        [MaxLength(10)]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Country")]
        [MaxLength(50)]
        public string Caption { get; set; }
    }

}
