using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IBFSNetwork.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using IBFSNetwork.Models;

namespace IBFSNetwork.Controllers
{
    public class SearchController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
       
        public object MimeMapping { get; private set; }

        public SearchController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // Post: Search
      
        public IActionResult Index(string firstName, string lastName, string idNumber,
                                        int locationId, int stateId, string city, string address, string phone, string email, string company,string alias, int page = 1, int pagesize = 20)
        {

            DataService.SearchService obj = new DataService.SearchService(_context);
           // SearchResulttWithPage 
            var alerts = obj.GetSearchResult(firstName, lastName, idNumber,
                                        locationId, stateId, city, address, phone, email, company,alias,page, pagesize);
            alerts.UserId = _userManager.GetUserAsync(HttpContext.User).Result.Id;

            ViewBag.Locations = new SelectList(_context.Locations, "LocationId", "Caption",locationId);
            ViewBag.States = new SelectList(_context.LocationStates, "LocationStateId", "Caption",stateId);
            ViewBag.Address = new SelectList(_context.Fraudsters.Where(a=>a.Address.Length>0), "Address", "Address",address);
            ViewBag.City = city;
            ViewBag.Alias = alias;
            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;
            ViewBag.IDNumber = idNumber;
            ViewBag.Phone = phone;
            ViewBag.Email = email;
            ViewBag.Company = company;
            ViewBag.Alias = alias;


            return View(alerts);
        }

        // post: Search
        [HttpPost]
        public IActionResult loadSearch()
        {
            int locationId, stateId;
            locationId = Convert.ToInt32(Request.Form["locationId"].ToString());
            stateId = Convert.ToInt32(Request.Form["stateId"].ToString());
           
            int page = 1;
            page = Convert.ToInt32(Request.Form["page"].ToString());
            int pageSize = Convert.ToInt32(Request.Form["pagesize"].ToString());

            string firstName = Request.Form["firstName"].ToString();
            string lastName = Request.Form["lastName"].ToString();
            string idNumber = Request.Form["idNumber"].ToString();
            string city = Request.Form["city"].ToString();
            string address = Request.Form["address"].ToString();
            string phone = Request.Form["phone"].ToString();
            string email = Request.Form["email"].ToString();
            string company = Request.Form["company"].ToString();
            string alias = Request.Form["alias"].ToString();

            DataService.SearchService obj = new DataService.SearchService(_context);
            // SearchResulttWithPage 
            var alerts = obj.GetSearchResult(firstName, lastName, idNumber,
                                        locationId, stateId, city, address, phone, email, company, alias, page, pageSize);

            return PartialView("_pSearch", alerts);
        }

    }
}