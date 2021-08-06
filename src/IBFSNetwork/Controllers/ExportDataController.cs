using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IBFSNetwork.Models;
using IBFSNetwork.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System;

namespace IBFSNetwork.Controllers
{
    public class myFileResult : ActionResult
    {
        public myFileResult(string fileDownloadName, string filePath, string contentType)
        {
            FileDownloadName = fileDownloadName;
            FilePath = filePath;
            ContentType = contentType;
        }

        public string ContentType { get; private set; }
        public string FileDownloadName { get; private set; }
        public string FilePath { get; private set; }

        public async override Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;    
            response.ContentType = ContentType;
            context.HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + FileDownloadName });
            using (var fileStream = new FileStream(FilePath, FileMode.Open))
            {
                await fileStream.CopyToAsync(context.HttpContext.Response.Body);
            }
        }
    }

    public class ExportDataController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationAdoContext _adocontext;

        public ExportDataController(ApplicationDbContext context, IHostingEnvironment environment, UserManager<ApplicationUser> userManager, ApplicationAdoContext adoContext)
        {
            // E:\papa\Dmitry\IBFSNetwork - BEforeCo-Co\src\IBFSNetwork\bin\Debug\netcoreapp1.0
           
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _adocontext = adoContext;
        }

        public virtual myFileResult File(string fileDownloadName, string filePath, string contentType = "application/octet-stream")
        {
            return new myFileResult(fileDownloadName, filePath, contentType);
        }

        public ActionResult GetCSV()
        {
            var data = ConvertToCsv();
            var bytes = UnicodeEncoding.Unicode.GetBytes(data);
            return File(bytes, "text/csv", "Fraudsters.csv");
        }

        public IActionResult GetXML()
        {
           // return File("test_001.xml", Path.Combine(_environment.WebRootPath, "uploads" + "\\test_001.xml"), "application/xml");
            var data = GetJsonData();
            string result = JsonConvert.SerializeObject(data, Formatting.None);
            Newtonsoft.Json.Converters.XmlNodeConverter con = new Newtonsoft.Json.Converters.XmlNodeConverter();
            XDocument doc = JsonConvert.DeserializeXNode("{\"Fraudsters\":\"Fraudster\":" + result + "}", "Fraudster");
            var bytes = UnicodeEncoding.Unicode.GetBytes(doc.ToString());
            return File(bytes, "application/xml", "Fraudsters.xml");
        }
  
        public IActionResult GetJson()
        {
            var data = GetJsonData();
            string result = JsonConvert.SerializeObject(data, Formatting.None);
            var bytes = UnicodeEncoding.Unicode.GetBytes(result);
            return File(bytes, "text/json", "Fraudsters.json");
        }
        //
        public IActionResult ExportDel()
        {
            //This GetAllData method will fetch data from server and create a comma seperate string.
            var data = ConvertToCsv();
            var bytes = UnicodeEncoding.Unicode.GetBytes(data);
            return File(bytes, "text/csv","testc.csv");
        }

        private string ConvertToCsv()
        {
            int maxIDs = GetMaxIDsCount();
            StringBuilder sb = new StringBuilder();
            var data = GetData();
            int id = 0;
            //Unsert header
            sb.Append("FraudsterId,");
            sb.Append("Last Name,");
            sb.Append("First Name,");
            sb.Append("MiddleName,");
            sb.Append("BOD,");
            sb.Append("Gender,");
            sb.Append("Company,");
            sb.Append("Address,");
            sb.Append("Alias,");
            sb.Append("PhoneNumber,");
            sb.Append("Email,");
            for (int i = 1; i <= maxIDs; i++)
            {
                sb.Append("IDType" + i.ToString() + ",");
                sb.Append("IDNumber" + i.ToString() + ",");
                sb.Append("IssuingCountry" + i.ToString() + ",");
                sb.Append("IssuingAuthority" + i.ToString() + ",");
                sb.Append("DateOfIssue" + i.ToString() + ",");
                sb.Append("ExpirationDate" + i.ToString() + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.AppendLine();
                int counter = 0;
            foreach (var row in data)
            {
                if (id != row.FraudsterId)
                {
                    if (id > 0)
                        {
                            if (counter <maxIDs)
                            {
                                for (int j= counter+1;j<=maxIDs; j++)
                                {
                                    for (int k=1;k<5; k++)
                                    sb.Append(",\"\"");
                                 }
                            }
                            sb.AppendLine();
                        }
                    counter = 1;

                    sb.Append("\""+row.FraudsterId.ToString()).Append("\",");
                    sb.Append("\"" + row.LastName + "\",");
                    sb.Append("\"" + row.FirstName + "\",");
                    sb.Append("\"" + row.MiddleName + "\",");
                    sb.Append("\"" + row.BOD.ToString("MM/dd/yyyy") + "\",");
                    sb.Append("\"" + row.Gender + "\",");
                    sb.Append("\"" + row.Company + "\",");
                    sb.Append("\"" + row.Address + "\",");
                    sb.Append("\"" + row.Alias + "\",");
                    sb.Append("\"" + row.PhoneNumber + "\",");
                    sb.Append("\"" + row.Email + "\",");

                    sb.Append("\"" + row.IDType + "\",");
                    sb.Append("\"" + row.PassportNumber + "\",");
                    sb.Append("\"" + row.IssuingCountry + "\",");
                    sb.Append("\"" + row.IssuingAuthority + "\",");
                    sb.Append("\"" + (row.DateOfIssue.HasValue ? row.DateOfIssue.Value.ToString("MM/dd/yyyy"):"") + "\",");
                    sb.Append("\"" + (row.ExpirationDate.HasValue ? row.ExpirationDate.Value.ToString("MM/dd/yyyy"):"")+ "\"");
                    id = row.FraudsterId;

                }
                else
                {
                        counter++;
                    sb.Append(",\"" + row.IDType + "\",\"");
                    sb.Append(row.PassportNumber + "\",\"");
                    sb.Append(row.IssuingCountry + "\",\"");
                    sb.Append(row.IssuingAuthority + "\",");
                    sb.Append("\"" + (row.DateOfIssue.HasValue ? row.DateOfIssue.Value.ToString("MM/dd/yyyy") : "") + "\",");
                    sb.Append("\"" + (row.ExpirationDate.HasValue ? row.ExpirationDate.Value.ToString("MM/dd/yyyy") : "")+"\"");

                }

            }
            if (id > 0)
                {
                    if (counter < maxIDs)
                    {
                        for (int j = counter + 1; j <= maxIDs; j++)
                        {
                            for (int k = 1; k < 5; k++)
                                sb.Append(",\"\"");
                        }
                    }
                        sb.AppendLine();
                }
                return sb.ToString();
        }

        private int GetMaxIDsCount()
        {
            //int result = 0;
            int result = (from frid in _context.FraudsterIDs
                         group frid by frid.FraudsterId into g
                         select new { g.Key, Count = g.Count() }).Max(c=>c.Count);

            return result;
        } 
  
        private List<Models.AlertViewModels.FraudsterWithID> GetData()
        {
            var model = from fr1 in _context.Fraudsters
                        join d in _context.FraudsterIDs on fr1.FraudsterId equals d.FraudsterId
                        join t in _context.IDTypes on d.IDTypeId equals t.IDTypeId
                        orderby fr1.FraudsterId
                        select new Models.AlertViewModels.FraudsterWithID
                        {
                            FraudsterId = fr1.FraudsterId,
                            FirstName = fr1.FirstName,
                            LastName = fr1.LastName,
                            MiddleName = fr1.MiddleName,
                            BOD = fr1.BOD,
                            Gender = fr1.Gender,
                            Address = fr1.Address,
                            Company = fr1.Company,
                            Alias = fr1.Alias,
                            Email = fr1.Email,
                            PhoneNumber = fr1.PhoneNumber,

                            IDType = t.IDTypeName,
                            PassportNumber = d.PassportNumber,
                            DateOfIssue = d.DateOfIssue,
                            ExpirationDate = d.ExpirationDate,
                            IssuingAuthority = d.IssuingAuthority,
                            IssuingCountry = d.IssuingCountry
                        };
            
            return model.ToList();
        }

        private List<Models.AlertViewModels.FraudsterJson> GetJsonData()
        {
            DataService.FraudsterService obj = new DataService.FraudsterService(_context);
            return obj.GetFraudstersForJson();
        }

   

    }
}
