using IBFSNetwork.Data;
using IBFSNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IBFSNetwork.DataService
{

    public class GetChartModels
    {
        private readonly ApplicationDbContext _context;

        public GetChartModels(ApplicationDbContext context)
        {
            _context = context;
        }

        // total loss bymonth
        public GoogleVisualizationDataTable GetTotalLossByMonth()
        {
            return GetDataTable(GetIndustryLossDB());
        }

        // fraudTrend
        public GoogleVisualizationDataTable GetFraudTrendByMonth()
        {
            //return GetDataTableForFraudCount(GetIndustryFraudCount());
            return GetDataTableForFraudCount(GetFraudCountByType());
        }
        private List<IndustryLoss> GetIndustryLossDBOLD()
        {
            int cnt = _context.Alerts.Count();
            var query = (from al in _context.Alerts
                         group al by new { year = al.AlertDate.Year, month = al.AlertDate.ToString("MMM"), monthId= al.AlertDate.Month} into d
                         select new IndustryLoss
                         {  Year = d.Key.ToString(), MonthId =d.Key.monthId, Month = d.Key.month.ToString(), TotalLoss = d.Sum(p => p.LostAmount), }).ToList().OrderBy(y=> y.Year).ThenBy(m=> m.MonthId).Take(6);


            //var group = from a in query
            //            group a by new { year1 = a.Year, month1 = a.Month, a.TotalLoss } into g
            //            select new IndustryLoss
            //            { Year = g.Key.year1.ToString(), Month = g.Key.month1.ToString(), TotalLoss = g.Sum(p => p.TotalLoss), };

            //IndustryLoss[] res = new IndustryLoss[query.Count()];

            ////

            //int i = 0;
            //IndustryLoss row;
            //foreach (var item1 in group)
            //{
            //    row = new IndustryLoss();
            //    row.Year = item1.Year;
            //    row.Month = item1.Month;
            //    row.TotalLoss = item1.TotalLoss;
            //    res[i] = row;
            //    i++;
            //}
            //return res;
            ////
            return query.ToList<IndustryLoss>();

        }

        private List<IndustryLoss> GetIndustryLossDB()
        {
            //int cnt = _context.Alerts.Count();
            var query = from ml in CurrentMonthList()
                        join al in _context.Alerts.ToList()
                        on new { year = ml.Year, month = ml.MonthId } equals new { year = al.AlertDate.Year, month = al.AlertDate.Month } into j1
                        from j2 in j1.DefaultIfEmpty()
                        group j2 by new { year = ml.Year, monthid = ml.MonthId, month = ml.MonthName } into grouped
                        select new IndustryLoss
                        {
                            Year = grouped.Key.year.ToString(),
                            MonthId = grouped.Key.monthid,
                            Month = grouped.Key.month,
                            TotalLoss = grouped.Where(g=> g !=null).Sum(p => p.LostAmount)
                        };

            var queryold = (from al in _context.Alerts
                         group al by new { year = al.AlertDate.Year, month = al.AlertDate.ToString("MMM"), monthId = al.AlertDate.Month } into d
                         select new IndustryLoss
                         { Year = d.Key.ToString(), MonthId = d.Key.monthId, Month = d.Key.month.ToString(), TotalLoss = d.Sum(p => p.LostAmount), }).ToList().OrderBy(y => y.Year).ThenBy(m => m.MonthId).Take(6);
            return query.ToList<IndustryLoss>();

        }
        private List<IndustryLoss> GetIndustryLossDBCopy()
        {
            int cnt = _context.Alerts.Count();
            var query = (from al in _context.Alerts
                         group al by new { year = al.AlertDate.Year, month = al.AlertDate.Month, mName = al.AlertDate.ToString("MMM") } into d
                         select new IndustryLoss
                         { Year = d.Key.ToString(), Month = d.Key.month.ToString(), TotalLoss = d.Sum(p => p.LostAmount), }).ToList();


            //var group = from a in query
            //            group a by new { year1 = a.Year, month1 = a.Month, a.TotalLoss } into g
            //            select new IndustryLoss
            //            { Year = g.Key.year1.ToString(), Month = g.Key.month1.ToString(), TotalLoss = g.Sum(p => p.TotalLoss), };

            //IndustryLoss[] res = new IndustryLoss[query.Count()];

            ////

            //int i = 0;
            //IndustryLoss row;
            //foreach (var item1 in group)
            //{
            //    row = new IndustryLoss();
            //    row.Year = item1.Year;
            //    row.Month = item1.Month;
            //    row.TotalLoss = item1.TotalLoss;
            //    res[i] = row;
            //    i++;
            //}
            //return res;
            ////
            return query.ToList<IndustryLoss>();

        }

        private GoogleVisualizationDataTable GetDataTable(List<IndustryLoss> data)
        {
            var dataTable = new GoogleVisualizationDataTable();

            // Get distinct markets from the data
            var months = data.Select(x => x.Month);

            // Get distinct years from the data
            //var years = data.Select(x => x.Year).Distinct().OrderBy(x => x);

            // Specify the columns for the DataTable.
            // In this example, it is Market and then a column for each year.
            dataTable.AddColumn("Month", "string");
            dataTable.AddColumn("IndustryLoss", "number");
            foreach (var rec in data)
            {
                var values = new List<object>(new[] { rec.Month});
                values.Add( (rec.TotalLoss/1000000).ToString() );

                dataTable.AddRow(values);
            }
            return dataTable;
        }

        //Trend
        
        private List<IndustryFraudCount> GetIndustryFraudCountOLD()
        {
            int cnt = _context.Alerts.Count();
            var query = (from al in _context.Alerts
                         group al by new { year = al.AlertDate.Year, month = al.AlertDate.ToString("MMM"), monthid = al.AlertDate.Month } into d
                         select new IndustryFraudCount
                         { Year = d.Key.ToString(), Month = d.Key.month.ToString(), MonthId =Convert.ToInt32( d.Key.monthid), FraudCount = d.Count() }).ToList().OrderBy(y=>y.Year).ThenBy(m=>m.MonthId).Take(6);

            return query.ToList<IndustryFraudCount>();

        }


        private List<IndustryFraudCount> GetIndustryFraudCount()
        {
      
            //int cnt = _context.Alerts.Count();
            var query = from ml in CurrentMonthList()
                        join al in _context.Alerts.ToList()
                        on new { year = ml.Year, month = ml.MonthId } equals new { year = al.AlertDate.Year, month = al.AlertDate.Month } into j1
                        from j2 in j1.DefaultIfEmpty()
                        group j2 by new { year = ml.Year, monthid = ml.MonthId, month = ml.MonthName } into grouped
                        select new IndustryFraudCount
                        {
                            Year =grouped.Key.year.ToString(),
                            MonthId = grouped.Key.monthid,
                            Month = grouped.Key.month,
                            FraudCount = grouped.Count(c=>c!=null)
                        };
           return query.ToList<IndustryFraudCount>();

        }

        //new
        private List<IndustryFraudCount> GetFraudCountByType()
        {

            //int cnt = _context.Alerts.Count();
            var query = from ft in _context.FraudTypes
                        join al in _context.Alerts on ft.FraudTypeId equals al.FraudTypeId into j1
                     //   from j2 in j1.DefaultIfEmpty()
                        select new IndustryFraudCount
                        {
                       //     Year = grouped.Key.year.ToString(),
                            MonthId = ft.FraudTypeId,
                            Month = ft.Caption,
                            FraudCount = j1.Count()
                        };
            return query.ToList<IndustryFraudCount>();

        }

        private GoogleVisualizationDataTable GetDataTableForFraudCount(List<IndustryFraudCount> data)
        {
            var dataTable = new GoogleVisualizationDataTable();

            // Get distinct markets from the data
            //var months = data.Select(x => x.Month).Distinct();

            
            // Specify the columns for the DataTable.
            dataTable.AddColumn("Month", "string");
            dataTable.AddColumn("FraudCount", "number");
            
            foreach (var fr in data )
            {
                var values = new List<object>(new[] { fr.Month});

                values.Add(fr.FraudCount.ToString());

                dataTable.AddRow(values);
            }
            return dataTable;
        }

        private class MonthList
        {
            public int Year { get; set; }
            public int MonthId { get; set; }
            public string MonthName { get; set; }
            public int Value { get; set; }

        }
        private List<MonthList> CurrentMonthList()
        {
            List<MonthList> lst = new List<MonthList>();
            MonthList month;
            DateTime dd;
            for (int i = 5; i >-1; i--)
            {
                dd = DateTime.Today.AddMonths(-i);
                month = new MonthList();
                month.MonthId = dd.Month;
                month.MonthName = dd.ToString("MMM");
                month.Year = dd.Year;
                lst.Add(month);
            }
            return lst;
        }

    }
}
