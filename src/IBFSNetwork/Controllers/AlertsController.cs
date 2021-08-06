using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBFSNetwork.Data;
using IBFSNetwork.Models.AlertViewModels;
using Microsoft.AspNetCore.Identity;
using IBFSNetwork.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace IBFSNetwork.Controllers
{
    public class AlertsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ApplicationAdoContext _adocontext;
        private IHostingEnvironment _environment;

        public object MimeMapping { get; private set; }

        static List<AutocomleteFraudster> Fraudsters = new List<AutocomleteFraudster>();

        /// <summary>
        /// Сonstructor
        /// </summary>
        public AlertsController(ApplicationDbContext context, ApplicationAdoContext adocontext, UserManager<ApplicationUser> userManager, IHostingEnvironment environment)
        {
            _context = context;
            _adocontext = adocontext;
            _userManager = userManager;
            _environment = environment;

            DataService.FraudsterService frs = new DataService.FraudsterService(_context);
            Fraudsters = frs.GetFraudsterTerms();
                        
        }

        public ActionResult AutocompleteSearch(string term)
        {
            var models = Fraudsters.Where(f=>f.FullName.Contains(term))
                .Select(a => new { value = a.FullName })
                .Distinct();
            return Json(models);
        }

        /// <summary>
        /// Filter result
        /// </summary>
        /// <param></param>
        /// 
        public IActionResult Index(int location, int countries, int state, string dateSortDirection, string locationSortDirection, string city = "",int page=1, int pagesize=20)
        {
            DataService.AlertService obj = new DataService.AlertService(_context,_adocontext);
           
            var alerts = obj.GetFilteredAlertsWithPage(location, countries, state, dateSortDirection, locationSortDirection, city,page,pagesize);
            alerts.UserId = _userManager.GetUserAsync(HttpContext.User).Result.Id;

            ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "Caption", location);
            ViewBag.LocationsState = new SelectList(_context.LocationStates, "LocationStateId", "Caption", state);
            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", "Caption", countries);

            ViewBag.City = city;
            return View(alerts);
        }
       
        // GET: Alerts
        public IActionResult IndexByAlert(int id, int page = 1, int pagesize = 20)
        {
            DataService.AlertService obj = new DataService.AlertService(_context,_adocontext);

            var alerts = obj.GetAlertsWithPageById(id, page, pagesize);

            ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "Caption");
            ViewBag.LocationsState = new SelectList(_context.LocationStates, "LocationStateId", "Caption");
            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", "Caption");
            ViewBag.City = "";
            return View("Index", alerts);
        }

        // post: Alerts
        [HttpPost]
        public IActionResult loadPage()
        {
            int location, countries, state;
            string city = "";
            int page = 1;
            location = Convert.ToInt32(Request.Form["location"].ToString());
            countries = 0;// Convert.ToInt32(Request.Form["countries"].ToString());
            state = Convert.ToInt32(Request.Form["state"].ToString());
            page = Convert.ToInt32(Request.Form["page"].ToString());
            int pageSize = Convert.ToInt32(Request.Form["pagesize"].ToString());

            city = Request.Form["city"].ToString();
            
            string datesortdirection = Request.Form["dateSortDirection"].ToString();
            string locationsortdirection = Request.Form["locationSortDirection"].ToString();
            //int pageSize = 5; // ?????????? ???????? ?? ????????

            DataService.AlertService obj = new DataService.AlertService(_context,_adocontext);
            //test

            var alerts = obj.GetFilteredAlertsWithPage(location, countries, state, datesortdirection, locationsortdirection, city, page, pageSize);
            
            return PartialView("_page", alerts);
        }

        // GET: Alerts/Create
        public async Task<IActionResult> Details(int? id )
        {
            ViewBag.BankSizes = new SelectList(_context.BankSizes, "BankSizeId", "Caption");
            ViewBag.BankTypes = new SelectList(_context.BankTypes, "BankTypeId", "Caption");
            ViewBag.FraudTypes = new SelectList(_context.FraudTypes, "FraudTypeId", "Caption");
            ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "Caption");
            ViewBag.LocationsByCircuit = new SelectList(_context.LocationByCircuits, "LocationByCircuitId", "Caption");
            ViewBag.LocationsState = new SelectList(_context.LocationStates, "LocationStateId", "Caption");

            ViewBag.IDTypes = new SelectList(_context.IDTypes, "IDTypeId", "IDTypeName");
           
            var alert = await _context.Alerts.SingleOrDefaultAsync(m => m.AlertId == id);
            if (alert == null)
            {
                return NotFound();
            }
            DataService.FraudsterService fs = new DataService.FraudsterService(_context);

            var frauds = fs.GetFraudstersByAlert(id.Value).ToList();  //_context.Fraudsters.Where(i => i.AlertId == id).OrderByDescending(o=>o.isMain).ToList();
            if (frauds.Count>0)
            {
                var fraudIDs = _context.FraudsterIDs.Include(i => i.IDType).Where(fi => fi.FraudsterId == frauds[0].FraudsterId);
                frauds[0].FraudsterIDs = fraudIDs.ToList();
            }
            var docs = _context.Documents.Where(d => d.FraudsterId== frauds[0].FraudsterId);           // alert.Fraudsters.Add(fraud);
            frauds[0].Documents = docs.ToList();
            alert.Fraudsters = frauds;
            return View(alert);
        }


        // GET: Alerts/Create
        public IActionResult Create()
        {
            ViewBag.BankSizes = new SelectList(_context.BankSizes, "BankSizeId", "Caption");
            ViewBag.BankTypes = new SelectList(_context.BankTypes, "BankTypeId", "Caption");
            ViewBag.FraudTypes = new SelectList(_context.FraudTypes, "FraudTypeId", "Caption");
            ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "Caption");
            ViewBag.LocationsByCircuit = new SelectList(_context.LocationByCircuits, "LocationByCircuitId", "Caption");
            ViewBag.LocationsState = new SelectList(_context.LocationStates, "LocationStateId", "Caption");
            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", "Caption");

            ViewBag.IDTypes = new SelectList(_context.IDTypes, "IDTypeId", "IDTypeName");
            ViewBag.AlertId = 0;
            return View();
        }

        // POST: Alerts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlertId,AlertDate,BankSizeId,BankTypeId,FraudTypeId,LocationId,LostAmount,Fraudsters,Documents, City,CountryId,Notes,LocationByCircuitId,LocationStateId")] Alert alert)
        {
            // Validate input date
            if (alert.BankTypeId ==0)
                ModelState.AddModelError("BankTypeId", "The BankType is requred");
            if (alert.BankSizeId == 0)
                ModelState.AddModelError("BankTypeId", "The BankSize is requred");
            if (alert.FraudTypeId == 0)
                ModelState.AddModelError("BankTypeId", "The FraudType is requred");
            if (alert.LocationId == 0)
                ModelState.AddModelError("BankTypeId", "The Location is requred");
            if (alert.LostAmount == 0)
                ModelState.AddModelError("LostAmount", "The Loss Amount is requred");
        
            if (alert.AlertDate == DateTime.MinValue)
                ModelState.AddModelError("Alert Date", "The Alert Date is requred");
        
            if (alert.Fraudsters[0].BOD ==DateTime.MinValue)
                ModelState.AddModelError("Fraudsters[0].BOD", "The DOB is requred");

            //  prepare documents
            var uploads = Path.Combine(_environment.WebRootPath, "uploads\\" + HttpContext.User.Identity.Name);
            if (alert.Fraudsters[0].Documents != null)
            {
                for (int i = 0; i < alert.Fraudsters[0].Documents.Count; i++)
                {
                        // Read the file and convert it to Byte Array
                    string filename = alert.Fraudsters[0].Documents[i].DocName;
                    string filePath = Path.Combine(uploads, filename);


                    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    br.Dispose();
                    fs.Dispose();
                    alert.Fraudsters[0].Documents[i].Content = bytes;
                    string ext = System.IO.Path.GetExtension(filename).ToLower();
                }
            }

            if (!ModelState.IsValid)
            {
                for (int i = 3; i > 0; i--)
                {
                    if (alert.Fraudsters[0].FraudsterIDs[i].IDTypeId == 0 && string.IsNullOrWhiteSpace(alert.Fraudsters[0].FraudsterIDs[i].PassportNumber))
                    {
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].IDTypeId");
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].DateOfIssue");
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].ExpirationDate");
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].IssuingAuthority");
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].IssuingCountry");
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].PassportNumber");

                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].PasportId");
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].FraudsterId");
                        alert.Fraudsters[0].FraudsterIDs.RemoveAt(i);

                    }
                }
            }
           

            if (ModelState.IsValid)
            {
                alert.ApplicationUserId= _userManager.GetUserAsync(HttpContext.User).Result.Id;
                if (alert.LocationByCircuitId == 0)
                    alert.LocationByCircuitId =null;
                if (alert.LocationStateId == 0)
                    alert.LocationStateId = null;
                if (alert.CountryId == 0)
                    alert.CountryId = null;
                                if (alert.Fraudsters[0].Documents != null)
                {
                    foreach (var doc in alert.Fraudsters[0].Documents)
                        doc.Fraudster = alert.Fraudsters[0];
                }
                // insert Alert Fraudster
                alert.AlertFraudster.isMain = true;
                alert.AlertFraudster.Alert = alert;
                alert.AlertFraudster.Fraudster = alert.Fraudsters[0];

                _context.Add(alert);
                
                await _context.SaveChangesAsync();
                Codes.Utils.DeleteDirectory(Path.Combine(_environment.WebRootPath, "uploads\\" + HttpContext.User.Identity.Name));
            }

            ViewBag.BankSizes = new SelectList(_context.BankSizes, "BankSizeId", "Caption", alert.BankSizeId);
            ViewBag.BankTypes = new SelectList(_context.BankTypes, "BankTypeId", "Caption", alert.BankTypeId);
            ViewBag.FraudTypes = new SelectList(_context.FraudTypes, "FraudTypeId", "Caption", alert.FraudTypeId);
            ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "Caption", alert.LocationId);
            ViewBag.FraudsterIDs = new SelectList(_context.FraudTypes, "FraudTypeId", "Caption", alert.FraudTypeId);
            ViewBag.LocationsByCircuit = new SelectList(_context.LocationByCircuits, "LocationByCircuitId", "Caption", alert.LocationByCircuitId);
            ViewBag.LocationsState = new SelectList(_context.LocationStates, "LocationStateId", "Caption", alert.LocationStateId);
            ViewBag.IDTypes = new SelectList(_context.IDTypes, "IDTypeId", "IDTypeName");
            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", "Caption",alert.CountryId);

            if (alert.Fraudsters[0].FraudsterIDs.Count < 4)
            {
                int idcnt = 4 - alert.Fraudsters[0].FraudsterIDs.Count;
                FraudsterID fid;
                for (int i = 0; i < idcnt; i++)
                {
                    fid = new FraudsterID();
                    alert.Fraudsters[0].FraudsterIDs.Add(fid);
                }
            }
            ViewBag.AlertId = alert.AlertId;
            return View(alert);
        }

        // GET: Alerts/Create
        public async Task<IActionResult> AddFraudster(int alertId)
        {
            ViewBag.BankSizes = new SelectList(_context.BankSizes, "BankSizeId", "Caption");
            ViewBag.BankTypes = new SelectList(_context.BankTypes, "BankTypeId", "Caption");
            ViewBag.FraudTypes = new SelectList(_context.FraudTypes, "FraudTypeId", "Caption");
            ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "Caption");
            ViewBag.LocationsByCircuit = new SelectList(_context.LocationByCircuits, "LocationByCircuitId", "Caption");
            ViewBag.LocationsState = new SelectList(_context.LocationStates, "LocationStateId", "Caption");
            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", "Caption");

            ViewBag.IDTypes = new SelectList(_context.IDTypes, "IDTypeId", "IDTypeName");
            ViewBag.FraudsterId = 0;
            var alert = await _context.Alerts.SingleOrDefaultAsync(m => m.AlertId == alertId);
            if (alert == null)
            {
                return NotFound();
            }
            Fraudster fr = new Fraudster();
            alert.Fraudsters.Add(fr);
            return View(alert);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFraudster(int alertId, [Bind("AlertId,AlertDate,BankSizeId,BankTypeId,FraudTypeId,LocationId,LostAmount,Fraudsters,Documents, City,CountryId,Notes,LocationByCircuitId,LocationStateId")] Alert alert)
        {
            // Validate input date
            if (alert.BankTypeId == 0)
                ModelState.AddModelError("BankTypeId", "The BankType is requred");
            if (alert.BankSizeId == 0)
                ModelState.AddModelError("BankTypeId", "The BankSize is requred");
            if (alert.FraudTypeId == 0)
                ModelState.AddModelError("BankTypeId", "The FraudType is requred");
            if (alert.LocationId == 0)
                ModelState.AddModelError("BankTypeId", "The Location is requred");
            if (alert.LostAmount == 0)
                ModelState.AddModelError("LostAmount", "The Loss Amount is requred");

            if (alert.AlertDate == DateTime.MinValue)
                ModelState.AddModelError("Alert Date", "The Alert Date is requred");

            if (alert.Fraudsters[0].BOD == DateTime.MinValue)
                ModelState.AddModelError("Fraudsters[0].BOD", "The DOB is requred");

            //  prepare documents
            var uploads = Path.Combine(_environment.WebRootPath, "uploads\\" + HttpContext.User.Identity.Name);
            if (alert.Fraudsters[0].Documents != null)
            {
                for (int i = 0; i < alert.Fraudsters[0].Documents.Count; i++)
                {
                    // Read the file and convert it to Byte Array
                    string filename = alert.Fraudsters[0].Documents[i].DocName;
                    string filePath = Path.Combine(uploads, filename);

                    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    br.Dispose();
                    fs.Dispose();
                    alert.Fraudsters[0].Documents[i].Content = bytes;
                    string ext = System.IO.Path.GetExtension(filename).ToLower();
                    //alert.Documents[i].Contentype = Codes.Mime.GetMimeType(filename);
                }
            }

            if (!ModelState.IsValid)
            {
                for (int i = 3; i > 0; i--)
                {
                    if (alert.Fraudsters[0].FraudsterIDs[i].IDTypeId == 0 && string.IsNullOrWhiteSpace(alert.Fraudsters[0].FraudsterIDs[i].PassportNumber))
                    {
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].IDTypeId");
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].DateOfIssue");
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].ExpirationDate");
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].IssuingAuthority");
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].IssuingCountry");
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].PassportNumber");

                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].PasportId");
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].FraudsterId");
                        alert.Fraudsters[0].FraudsterIDs.RemoveAt(i);

                    }
                }
            }

           
            if (ModelState.IsValid)
            {
                alert.ApplicationUserId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
                if (alert.LocationByCircuitId == 0)
                    alert.LocationByCircuitId = null;
                if (alert.LocationStateId == 0)
                    alert.LocationStateId = null;
                if (alert.CountryId == 0)
                    alert.CountryId = null;
                
               // alert.AlertDate = DateTime.Now;
                if (alert.Fraudsters[0].Documents != null)
                {
                    foreach (var doc in alert.Fraudsters[0].Documents)
                        doc.Fraudster = alert.Fraudsters[0];
                }
                // insert Alert Fraudster
                alert.AlertFraudster.Alert = alert;
                alert.AlertFraudster.Fraudster = alert.Fraudsters[0];

                _context.Update(alert);

                await _context.SaveChangesAsync();
                Codes.Utils.DeleteDirectory(Path.Combine(_environment.WebRootPath, "uploads\\" + HttpContext.User.Identity.Name));
                return RedirectToAction("Index");
            }

            ViewBag.BankSizes = new SelectList(_context.BankSizes, "BankSizeId", "Caption", alert.BankSizeId);
            ViewBag.BankTypes = new SelectList(_context.BankTypes, "BankTypeId", "Caption", alert.BankTypeId);
            ViewBag.FraudTypes = new SelectList(_context.FraudTypes, "FraudTypeId", "Caption", alert.FraudTypeId);
            ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "Caption", alert.LocationId);
            ViewBag.FraudsterIDs = new SelectList(_context.FraudTypes, "FraudTypeId", "Caption", alert.FraudTypeId);
            ViewBag.LocationsByCircuit = new SelectList(_context.LocationByCircuits, "LocationByCircuitId", "Caption", alert.LocationByCircuitId);
            ViewBag.LocationsState = new SelectList(_context.LocationStates, "LocationStateId", "Caption", alert.LocationStateId);
            ViewBag.IDTypes = new SelectList(_context.IDTypes, "IDTypeId", "IDTypeName");
            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", "Caption", alert.CountryId);

            if (alert.Fraudsters[0].FraudsterIDs.Count < 4)
            {
                int idcnt = 4 - alert.Fraudsters[0].FraudsterIDs.Count;
                FraudsterID fid;
                for (int i = 0; i < idcnt; i++)
                {
                    fid = new FraudsterID();
                    alert.Fraudsters[0].FraudsterIDs.Add(fid);
                }
            }
            ViewBag.FraudsterId = alert.Fraudsters[0].FraudsterId;
            return View(alert);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkFraudsters(int alertId, string term)
        {
            // Validate input date
            AlertFraudster alf = new Models.AlertViewModels.AlertFraudster();
            alf.AlertId = alertId;
            string[] str = term.Split(',');
            DataService.FraudsterService frs = new DataService.FraudsterService(_context);
            var fraudsterId = frs.GetFraudsterIdByName(str[1], str[0]);
            alf.FraudsterId = fraudsterId;
            _context.Add(alf);

                await _context.SaveChangesAsync();
            return RedirectToAction("IndexByAlert",new { id= alertId });
        }

        // GET: Alerts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.BankSizes = new SelectList(_context.BankSizes, "BankSizeId", "Caption");
            ViewBag.BankTypes = new SelectList(_context.BankTypes, "BankTypeId", "Caption");
            ViewBag.FraudTypes = new SelectList(_context.FraudTypes, "FraudTypeId", "Caption");
            ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "Caption");
            ViewBag.LocationsByCircuit = new SelectList(_context.LocationByCircuits, "LocationByCircuitId", "Caption");
            ViewBag.LocationsState = new SelectList(_context.LocationStates, "LocationStateId", "Caption");

            ViewBag.IDTypes = new SelectList(_context.IDTypes, "IDTypeId", "IDTypeName");

            var alert = await _context.Alerts.SingleOrDefaultAsync(m => m.AlertId == id);
            if (alert == null)
            {
                return NotFound();
            }

            DataService.FraudsterService fs = new DataService.FraudsterService(_context);
            var frauds = fs.GetFraudstersByAlert(id.Value).ToList(); //_context.Fraudsters.Where(i => i.AlertId == id).OrderByDescending(o => o.isMain).ToList();

            List<FraudsterID> fraudIDs = _context.FraudsterIDs.Include(i => i.IDType).Where(fi => fi.FraudsterId == frauds[0].FraudsterId).ToList();
            while (fraudIDs.Count() < 4)
            {
                FraudsterID fid = new FraudsterID();
                fraudIDs.Add(fid);
            }
            frauds[0].FraudsterIDs = fraudIDs.ToList();

            var docs = _context.Documents.Where(d =>  d.FraudsterId == frauds[0].FraudsterId);           // alert.Fraudsters.Add(fraud);
            alert.Fraudsters[0].Documents = docs.ToList();
            alert.Fraudsters = frauds;
            return View(alert);
        }

        // POST: Alerts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want tond to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AlertId,AlertDate,BankSizeId,BankTypeId,FraudTypeId,LocationId,LostAmount,Fraudsters,Documents, City,CountryId,Notes,LocationByCircuitId,LocationStateId")] Alert alert)
        {
            if (id != alert.AlertId)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                for (int i = 3; i > 0; i--)
                {
                    if (alert.Fraudsters[0].FraudsterIDs[i].IDTypeId == 0 && string.IsNullOrWhiteSpace(alert.Fraudsters[0].FraudsterIDs[i].PassportNumber))
                    {
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].IDTypeId");
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].DateOfIssue");
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].ExpirationDate");
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].IssuingAuthority");
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].IssuingCountry");
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].PassportNumber");

                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].PasportId");
                        ModelState.Remove("Fraudsters[0].FraudsterIDs[" + i.ToString() + "].FraudsterId");
                        alert.Fraudsters[0].FraudsterIDs.RemoveAt(i);

                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (alert.LocationByCircuitId == 0)
                        alert.LocationByCircuitId = null;
                    if (alert.LocationStateId == 0)
                        alert.LocationStateId = null;
                    if (alert.CountryId == 0)
                        alert.CountryId = null;
                    //alert.Fraudsters[0].isMain = true;

                    DataService.DocumentService obj = new DataService.DocumentService(_context);
                    List<string> l = new List<string>();
                    if (alert.Fraudsters[0].Documents!=null)
                    { 
                        foreach (var d in alert.Fraudsters[0].Documents)
                            l.Add(d.DocName.Replace("\r","").Replace("\n",""));
                    }
                    // delete doc
                    var dellist = obj.DocToDelete(alert.Fraudsters[0].FraudsterId, l);
                    // store existing
                    var storelist = obj.DocToStore(alert.Fraudsters[0].FraudsterId, l);
                    // delete from update list
                    

                    // delete from db
                    foreach (var d in dellist)
                        _context.Documents.Remove(d);

                    foreach (Document doc1 in storelist)
                    {
                        var st = ModelState.FirstOrDefault(v => v.Value.RawValue.ToString().Trim().Equals(doc1.DocName.Trim()));
                        var st1 = ModelState.Where(i=> i.Value.RawValue.ToString() == doc1.DocName);
                        var st2 = ModelState.FirstOrDefault(v => v.Value.AttemptedValue.Equals(doc1.DocName));
                        var st3 = ModelState.Single(s => s.Value.RawValue.ToString().Trim() == doc1.DocName.Trim());
                        

                        if (st.Key != null)
                        {//     ModelState.Remove(st.Key);
                            int start = st.Key.IndexOf("[");
                            int length = st.Key.IndexOf("]") - start - 1;
                            string docid = st.Key.Substring(start + 1, length);

                            //if (ModelState.FirstOrDefault(v => v.Key.Equals("a")).Key!=null)

                            ModelState.Remove("Documents[" + docid.ToString() + "].DocumentId");
                            ModelState.Remove("Documents[" + docid.ToString() + "].Contentype");
                            ModelState.Remove("Documents[" + docid.ToString() + "].DocName");
                            var index = alert.Fraudsters[0].Documents.FindIndex(i => i.DocName.Trim() == doc1.DocName.Trim());
                            if (index>=0)
                                alert.Fraudsters[0].Documents.RemoveAt(index);
                        }
                    }
                    // alert frausters
                    
                    //  prepare documents
                    var uploads = Path.Combine(_environment.WebRootPath, "uploads\\" + HttpContext.User.Identity.Name);
                    if (alert.Fraudsters[0].Documents != null)
                    {
                        for (int i = 0; i < alert.Fraudsters[0].Documents.Count; i++)
                        {
                            // Read the file and convert it to Byte Array
                            string filename = alert.Fraudsters[0].Documents[i].DocName;
                            string filePath = Path.Combine(uploads, filename);

                            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fs);
                            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                            br.Dispose();
                            fs.Dispose();

                            alert.Fraudsters[0].Documents[i].Content = bytes;
                            string ext = System.IO.Path.GetExtension(filename).ToLower();

                        }
                        foreach (var doc in alert.Fraudsters[0].Documents)
                            doc.FraudsterId = alert.Fraudsters[0].FraudsterId;
                    }

                    DataService.FraudsterService frservice = new DataService.FraudsterService(_context);
                    alert.AlertFraudster = frservice.GetAlertFraudsterModel(alert.AlertId, alert.Fraudsters[0].FraudsterId);

                    _context.Update(alert);
                    await _context.SaveChangesAsync();
                    Codes.Utils.DeleteDirectory(Path.Combine(_environment.WebRootPath, "uploads\\" + HttpContext.User.Identity.Name));

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlertExists(alert.AlertId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewBag.BankSizes = new SelectList(_context.BankSizes, "BankSizeId", "Caption");
            ViewBag.BankTypes = new SelectList(_context.BankTypes, "BankTypeId", "Caption");
            ViewBag.FraudTypes = new SelectList(_context.FraudTypes, "FraudTypeId", "Caption");
            ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "Caption");
            ViewBag.LocationsByCircuit = new SelectList(_context.LocationByCircuits, "LocationByCircuitId", "Caption");
            ViewBag.LocationsState = new SelectList(_context.LocationStates, "LocationStateId", "Caption");

            ViewBag.IDTypes = new SelectList(_context.IDTypes, "IDTypeId", "IDTypeName");
            DataService.FraudsterService frs = new DataService.FraudsterService(_context);
            var frauds = frs.GetMainFraudstersByAlert(id); //_context.Fraudsters.Where(i => i.AlertId == id).OrderByDescending(o => o.isMain).ToList();

            return View(alert);
        }

        // GET: Alerts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alert = await _context.Alerts.SingleOrDefaultAsync(m => m.AlertId == id);
            if (alert == null)
            {
                return NotFound();
            }

            return View(alert);
        }

        // POST: Alerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var alert = await _context.Alerts.SingleOrDefaultAsync(m => m.AlertId == id);
            _context.Alerts.Remove(alert);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RenderID()
        {

            // read list of previous uploaded files
            int cnt = Convert.ToInt32(Request.Form["itemcount"].ToString());
            ViewBag.Number = cnt;
            return PartialView("pID");

        }

        private bool AlertExists(int id)
        {
            return _context.Alerts.Any(e => e.AlertId == id);
        }

    }
}
