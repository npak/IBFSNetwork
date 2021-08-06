using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IBFSNetwork.Models;
using IBFSNetwork.Data;
using IBFSNetwork.DataService;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace IBFSNetwork.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationAdoContext _adocontext;

        public HomeController(ApplicationDbContext context, IHostingEnvironment environment, UserManager<ApplicationUser> userManager, ApplicationAdoContext adoContext)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _adocontext = adoContext;
        }

        // GET: Alerts
        public async Task<IActionResult> Index()
        {
            var alert = new Models.AlertViewModels.AlertDisplayModel();
            DataService.AlertService ser = new AlertService(_context,_adocontext);
            alert = ser.GetAlertDisplayModel();

            //add feeds  ADD AFTER
            var us1 = HttpContext.User;
            var res=_userManager.GetUserAsync(us1).Result;
            Codes.FeedParser fp = new Codes.FeedParser(_context, _userManager.GetUserAsync(HttpContext.User).Result.Id, User.IsInRole("admin"));
            alert.feeds = await fp.GetFeeds();

            //charts
            // Get the chart data from the database.  At this point it is just an array of MarketSales objects.
            GetChartModels chart = new GetChartModels(_context);

            //total fraud data
            var data = chart.GetTotalLossByMonth();
            var industryloss = new IndustryLossChartModel();
            industryloss.Title = "Total Industr Loss By Month";
            industryloss.DataTable  = data;
            alert.IndustyLoss = industryloss;

            //Trend  alerts
            var dataTrend = chart.GetFraudTrendByMonth();
            var industryTrend = new IndustryLossChartModel();
            industryTrend.Title = "Alerts Trend By Month";
            industryTrend.DataTable = dataTrend;
            alert.IndustyTrend = industryTrend;

            // Forum
            DataService.ForumService forum = new ForumService(_context,_adocontext);
            var forumList = forum.GetLast10();
            alert.Questions = forumList;
            return View("Index",alert);
        }


        // GET: Alerts
        public async Task<IActionResult> FraudTrendChart()
        {
            var alert = new Models.AlertViewModels.AlertDisplayModel();
            
            //charts
            // Get the chart data from the database.  At this point it is just an array of MarketSales objects.
            GetChartModels chart = new GetChartModels(_context);

            
            //Trend  alerts
            var dataTrend = chart.GetFraudTrendByMonth();
            var industryTrend = new IndustryLossChartModel();
            industryTrend.Title = "Alerts Trend By Month";
            industryTrend.DataTable = dataTrend;
            alert.IndustyTrend = industryTrend;

            return View("FraudTrendChart", alert);
        }

        // GET: Alerts
        public async Task<IActionResult> TotalLossChart()
        {
            var alert = new Models.AlertViewModels.AlertDisplayModel();
           
            //charts
            // Get the chart data from the database.  At this point it is just an array of MarketSales objects.
            GetChartModels chart = new GetChartModels(_context);

            //total fraud data
            var data = chart.GetTotalLossByMonth();
            var industryloss = new IndustryLossChartModel();
            industryloss.Title = "Total Industr Loss By Month";
            industryloss.DataTable = data;
            alert.IndustyLoss = industryloss;

            //Trend  alerts
            //var dataTrend = chart.GetFraudTrendByMonth();
            //var industryTrend = new IndustryLossChartModel();
            //industryTrend.Title = "Alerts Trend By Month";
            //industryTrend.DataTable = dataTrend;
            //alert.IndustyTrend = industryTrend;

            return View("TotalLossChart", alert);
        }


        [HttpPost]
        public async Task<IActionResult> Index(ICollection<IFormFile> files)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);

                    }
                }
            }
            return View();
        }

        //public void ExportListFromTsv()
        //{
        //    var data = new[]{
        //                       new{ Name="Ram", Email="ram@techbrij.com", Phone="111-222-3333" },
        //                       new{ Name="Shyam", Email="shyam@techbrij.com", Phone="159-222-1596" },
        //                       new{ Name="Mohan", Email="mohan@techbrij.com", Phone="456-222-4569" },
        //                       new{ Name="Sohan", Email="sohan@techbrij.com", Phone="789-456-3333" },
        //                       new{ Name="Karan", Email="karan@techbrij.com", Phone="111-222-1234" },
        //                       new{ Name="Brij", Email="brij@techbrij.com", Phone="111-222-3333" }
        //              };

        //    Response.Clear();
        //    Response.Headers.Add("content-disposition", "attachment;filename=Contact.xls");
        //    Response.Headers.Add("Content-Type", "application/vnd.ms-excel");

        //    Codes.Data_ToXls obj = new Codes.Data_ToXls();
        //    obj.WriteTsv(data, Response.Output);
        //    Response.end();
        //}
        //public HttpResponseMessage Get(int id)
        //{
        //    MemoryStream stream = new MemoryStream();
        //    StreamWriter writer = new StreamWriter(stream);
        //    writer.Write("Hello, World!");
        //    writer.Flush();
        //    stream.Position = 0;

        //    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
        //    result.Content = new StreamContent(stream);
        //    result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/csv");
        //    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = "Export.csv" };
        //    return result;
        //}

        //[HttpGet]
        //public FileStreamResult DownloadFile()
        //{
        //    Response.Headers.Add("content-disposition", "attachment; filename=test.rar");
        //    return File(new FileStream(@"G:\test.rar", FileMode.Open),
        //                "application/octet-stream"); // or "application/x-rar-compressed"
        //}

        //public ActionResult Get1(int id)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    var frs = _context.Fraudsters;
        //    foreach (Models.AlertViewModels.Fraudster fr in frs)
        //    {
        //        sb.AppendFormat(
        //            "{0};{1};{2};",

        //            fr.FirstName,
        //            fr.LastName,
        //            Environment.NewLine);
        //    }

        //    MemoryStream stream = new MemoryStream();
        //    StreamWriter writer = new StreamWriter(stream);
        //    writer.Write(sb.ToString());
        //    writer.Flush();
        //    stream.Position = 0;

        //    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
        //    result.Content = new StreamContent(stream);
        //    result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/csv");
        //    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = "Export.csv" };
        //    result.Dispose();
        //    return View("Index");
        //}



        //public ActionResult DumpToCSV(string data)
        //{
        //    //Response.Clear();

        //    //XmlNode xml = JsonConvert.DeserializeXmlNode("{records:{record:" + data + "}}");

        //    //XmlDocument xmldoc = new XmlDocument();
        //    ////Create XmlDoc Object
        //    //xmldoc.LoadXml(xml.InnerXml);
        //    ////Create XML Steam 
        //    //var xmlReader = new XmlNodeReader(xmldoc);
        //    //DataSet dataSet = new DataSet();
        //    ////Load Dataset with Xml
        //    //dataSet.ReadXml(xmlReader);
        //    ////return single table inside of dataset
        //    //var csv = CustomReportBusinessModel.ToCSV(dataSet.Tables[0], ",");

        //    //HttpContext context = System.Web.HttpContext.Current;
        //    //context.Response.Write(csv);
        //    //context.Response.ContentType = "text/csv";
        //    //context.Response.AddHeader("Content-Disposition", "attachment;filename=Custom Report.csv");
        //    //Response.End();
        //    return null;
        //}
        //[HttpPost]
        [HttpGet]
        public IActionResult LoadID()
        {
            ViewBag.Number = 10; 
                //Convert.ToInt32(Request.Form["number"].ToString());
            return PartialView("pID");
        }

    }
}
