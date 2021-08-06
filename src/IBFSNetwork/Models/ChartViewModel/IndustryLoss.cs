using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace IBFSNetwork.Models
{
    public class IndustryLoss
    {
        public string Year { get; set; }
        public int MonthId { get; set; }
        public string Month { get; set; }
        public int TotalLoss { get; set; }

    }

    public class IndustryFraudCount
    {
        public string Year { get; set; }
        public int MonthId { get; set; }
        public string Month { get; set; }
        public int FraudCount { get; set; }

    }
    //
    public class IndustryLossChartModel
    {
        public string Title { get; set; }
        public GoogleVisualizationDataTable DataTable { get; set; }
        //public IndustryLoss[] DataList { get; set; }
    }



}

