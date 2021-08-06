using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBFSNetwork.Data; 
using IBFSNetwork.Models.AlertViewModels;
using Microsoft.AspNetCore.Authorization;

namespace IBFSNetwork.Controllers
{
    //[Authorize(Roles = "admin")]

    public class FraudstersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FraudstersController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Fraudsters
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Fraudsters;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Fraudsters
        public IActionResult FraudstersByAlert(int id)
        {
            DataService.FraudsterService fs = new DataService.FraudsterService(_context);
            var applicationDbContext = fs.GetFraudstersByAlert(id);
            ViewBag.AlertId = id;
            return View(fs.GetFraudstersByAlert(id).ToList());
        }
        // GET: Fraudsters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fraudster = await _context.Fraudsters.SingleOrDefaultAsync(m => m.FraudsterId == id);
            if (fraudster == null)
            {
                return NotFound();
            }

            return View(fraudster);
        }

        // GET: Fraudsters/Create
        public IActionResult Create(int id)
        {
            ViewBag.AlertId = id.ToString();
            ViewBag.IDTypes = new SelectList(_context.IDTypes, "IDTypeId", "IDTypeName");

            return View();
        }

        // POST: Fraudsters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FraudsterId,BOD,Company,FirstName,Gender,LastName,MiddleName,PhoneNumber,Address,Email,FraudsterIDs")] Fraudster fraudster)
        {
            if (fraudster.FraudsterIDs[0].IDTypeId == 0 && string.IsNullOrWhiteSpace(fraudster.FraudsterIDs[0].PassportNumber))
            {
                ModelState.AddModelError("ID", "ID type and Id number are requred");
            }

            if (!ModelState.IsValid)
            {
                if (fraudster.FraudsterIDs[3].IDTypeId == 0 && string.IsNullOrWhiteSpace(fraudster.FraudsterIDs[3].PassportNumber))
                {
                    ModelState.Remove("FraudsterIDs[3].IDTypeId");
                    ModelState.Remove("FraudsterIDs[3].DateOfIssue");
                    ModelState.Remove("FraudsterIDs[3].ExpirationDate");
                    ModelState.Remove("FraudsterIDs[3].IssuingAuthority");
                    ModelState.Remove("FraudsterIDs[3].IssuingCountry");
                    ModelState.Remove("FraudsterIDs[3].PassportNumber");

                    ModelState.Remove("FraudsterIDs[3].PasportId");
                    ModelState.Remove("FraudsterIDs[3].FraudsterId");
                    fraudster.FraudsterIDs.RemoveAt(3);

                }

                if (fraudster.FraudsterIDs[2].IDTypeId == 0 && string.IsNullOrWhiteSpace(fraudster.FraudsterIDs[2].PassportNumber))
                {
                    ModelState.Remove("FraudsterIDs[2].IDTypeId");
                    ModelState.Remove("FraudsterIDs[2].DateOfIssue");
                    ModelState.Remove("FraudsterIDs[2].ExpirationDate");
                    ModelState.Remove("FraudsterIDs[2].IssuingAuthority");
                    ModelState.Remove("FraudsterIDs[2].IssuingCountry");
                    ModelState.Remove("FraudsterIDs[2].PassportNumber");
                    ModelState.Remove("FraudsterIDs[2].PasportId");
                    ModelState.Remove("FraudsterIDs[2].FraudsterId");
                    fraudster.FraudsterIDs.RemoveAt(2);
                }

                if (fraudster.FraudsterIDs[1].IDTypeId == 0 && string.IsNullOrWhiteSpace(fraudster.FraudsterIDs[1].PassportNumber))
                {
                    ModelState.Remove("FraudsterIDs[1].IDTypeId");
                    ModelState.Remove("FraudsterIDs[1].DateOfIssue");
                    ModelState.Remove("FraudsterIDs[1].ExpirationDate");
                    ModelState.Remove("FraudsterIDs[1].IssuingAuthority");
                    ModelState.Remove("FraudsterIDs[1].IssuingCountry");
                    ModelState.Remove("FraudsterIDs[1].PassportNumber");
                    ModelState.Remove("FraudsterIDs[1].PasportId");
                    ModelState.Remove("FraudsterIDs[1].FraudsterId");
                    fraudster.FraudsterIDs.RemoveAt(1);
                }
            }

            if (ModelState.IsValid)
            {

                _context.Add(fraudster);
                await _context.SaveChangesAsync();
                return RedirectToAction("Alerts", "Index");
            }
            if (fraudster.FraudsterIDs.Count < 4)
            {
                int cnt = 4 - fraudster.FraudsterIDs.Count;
                FraudsterID fid;
                for (int i = 0; i < cnt; i++)
                {
                    fid = new FraudsterID();
                    fraudster.FraudsterIDs.Add(fid);
                }


            }
            ViewBag.IDTypes = new SelectList(_context.IDTypes, "IDTypeId", "IDTypeName");

            return View("FraudstersByAlert");
        }

        // GET: Fraudsters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fraudster = await _context.Fraudsters.SingleOrDefaultAsync(m => m.FraudsterId == id);
            if (fraudster == null)
            {
                return NotFound();
            }
            ViewBag.IDTypes = new SelectList(_context.IDTypes, "IDTypeId", "IDTypeName");
            List<FraudsterID> fraudIDs = _context.FraudsterIDs.Include(i => i.IDType).Where(fi => fi.FraudsterId == id).ToList();
            while (fraudIDs.Count() < 4)
            {
                FraudsterID fid = new FraudsterID();
                fraudIDs.Add(fid);
            }
            fraudster.FraudsterIDs = fraudIDs.ToList();
            DataService.FraudsterService fs = new DataService.FraudsterService(_context);
            ViewBag.AlertId = fs.GetAlertIdByFraudsterId(id.Value);
            return View(fraudster);
        }

        // POST: Fraudsters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FraudsterId,BOD,Company,FirstName,Gender,LastName,MiddleName,PhoneNumber,Address,Email,FraudsterIDs")] Fraudster fraudster)
        {
            if (id != fraudster.FraudsterId)
            {
                return NotFound();
            }

            if (fraudster.FraudsterIDs[0].IDTypeId == 0 && string.IsNullOrWhiteSpace(fraudster.FraudsterIDs[0].PassportNumber))
            {
                ModelState.AddModelError("ID", "ID type and Id number are requred");
            }

            if (!ModelState.IsValid)
            {
                if (fraudster.FraudsterIDs[3].IDTypeId == 0 && string.IsNullOrWhiteSpace(fraudster.FraudsterIDs[3].PassportNumber))
                {
                    ModelState.Remove("FraudsterIDs[3].IDTypeId");
                    ModelState.Remove("FraudsterIDs[3].DateOfIssue");
                    ModelState.Remove("FraudsterIDs[3].ExpirationDate");
                    ModelState.Remove("FraudsterIDs[3].IssuingAuthority");
                    ModelState.Remove("FraudsterIDs[3].IssuingCountry");
                    ModelState.Remove("FraudsterIDs[3].PassportNumber");

                    ModelState.Remove("FraudsterIDs[3].PasportId");
                    ModelState.Remove("FraudsterIDs[3].FraudsterId");
                    fraudster.FraudsterIDs.RemoveAt(3);

                }

                if (fraudster.FraudsterIDs[2].IDTypeId == 0 && string.IsNullOrWhiteSpace(fraudster.FraudsterIDs[2].PassportNumber))
                {
                    ModelState.Remove("FraudsterIDs[2].IDTypeId");
                    ModelState.Remove("FraudsterIDs[2].DateOfIssue");
                    ModelState.Remove("FraudsterIDs[2].ExpirationDate");
                    ModelState.Remove("FraudsterIDs[2].IssuingAuthority");
                    ModelState.Remove("FraudsterIDs[2].IssuingCountry");
                    ModelState.Remove("FraudsterIDs[2].PassportNumber");
                    ModelState.Remove("FraudsterIDs[2].PasportId");
                    ModelState.Remove("FraudsterIDs[2].FraudsterId");
                    fraudster.FraudsterIDs.RemoveAt(2);
                }

                if (fraudster.FraudsterIDs[1].IDTypeId == 0 && string.IsNullOrWhiteSpace(fraudster.FraudsterIDs[1].PassportNumber))
                {
                    ModelState.Remove("FraudsterIDs[1].IDTypeId");
                    ModelState.Remove("FraudsterIDs[1].DateOfIssue");
                    ModelState.Remove("FraudsterIDs[1].ExpirationDate");
                    ModelState.Remove("FraudsterIDs[1].IssuingAuthority");
                    ModelState.Remove("FraudsterIDs[1].IssuingCountry");
                    ModelState.Remove("FraudsterIDs[1].PassportNumber");
                    ModelState.Remove("FraudsterIDs[1].PasportId");
                    ModelState.Remove("FraudsterIDs[1].FraudsterId");
                    fraudster.FraudsterIDs.RemoveAt(1);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fraudster);
                    await _context.SaveChangesAsync();
                    DataService.FraudsterService fs = new DataService.FraudsterService(_context);
                    var alertId = fs.GetAlertIdByFraudsterId(fraudster.FraudsterId);
                    return RedirectToAction("FraudstersByAlert", "Fraudsters", new { id = alertId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FraudsterExists(fraudster.FraudsterId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }

           if (fraudster.FraudsterIDs.Count < 4)
            {
                int cnt = 4 - fraudster.FraudsterIDs.Count;
                FraudsterID fid;
                for (int i = 0; i < cnt; i++)
                {
                    fid = new FraudsterID();
                    fraudster.FraudsterIDs.Add(fid);
                }


            }
            ViewBag.IDTypes = new SelectList(_context.IDTypes, "IDTypeId", "IDTypeName");

            return View(fraudster);
        }

        // GET: Fraudsters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fraudster = await _context.Fraudsters.SingleOrDefaultAsync(m => m.FraudsterId == id);
            if (fraudster == null)
            {
                return NotFound();
            }

            DataService.FraudsterService fs = new DataService.FraudsterService(_context);
            ViewBag.AlertId = fs.GetAlertIdByFraudsterId(id.Value);
            return View(fraudster);
        }

        // POST: Fraudsters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            DataService.FraudsterService fs = new DataService.FraudsterService(_context);
           var alertId = fs.GetAlertIdByFraudsterId(id);

            var fraudster = await _context.Fraudsters.SingleOrDefaultAsync(m => m.FraudsterId == id);
            _context.Fraudsters.Remove(fraudster);
            // delete docs
            var docs = await _context.Documents.Where(f => f.FraudsterId == id).ToListAsync();
            foreach (var doc in docs)
                 _context.Documents.Remove(doc);

            await _context.SaveChangesAsync();
            return RedirectToAction("FraudstersByAlert",new { id = alertId});
        }

        private bool FraudsterExists(int id)
        {
            return _context.Fraudsters.Any(e => e.FraudsterId == id);
        }
    }
}
