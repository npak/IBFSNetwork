using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IBFSNetwork.Data;
using IBFSNetwork.Models;
using Microsoft.EntityFrameworkCore;

namespace IBFSNetwork.Controllers
{
    public class SurveyAnonymousController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SurveyAnonymousController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            Codes.Crypto cr = new Codes.Crypto();
            string ss = "DislayEmail";
            string enctxt = cr.Encrypt(ss);
            string text = cr.Decrypt(enctxt);
            return View();
        }

        // GET: Questionnaires/Create
        [HttpGet]
        [AllowAnonymous]
        // GET: QuestionnaireModels/Create
        public IActionResult Create()
        {
            ViewBag.Questionnaire = _context.Questionnaires.Where(q => !q.Deleted).ToList();
            return View();
        }

        // POST: QuestionnaireModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create([Bind("Id,DateOn,Name,QuestionnaireAnswers")] QuestionnaireModel questionnaireModel)
        {
            if (ModelState.IsValid)
            {
                questionnaireModel.DateOn = DateTime.Now;
                _context.Add(questionnaireModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(questionnaireModel);
        }

        // GET: QuestionnaireModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionnaireModel = await _context.QuestionnaireModels.SingleOrDefaultAsync(m => m.Id == id);

            if (questionnaireModel == null)
            {
                return NotFound();
            }
            questionnaireModel.QuestionnaireAnswers = _context.QuestionnaireAnswers.Where(q => q.QuestionnaireModelId == id).ToList();
            ViewBag.Questionnaire = _context.Questionnaires.Where(q => !q.Deleted).ToList();
            return View(questionnaireModel);
        }
    }
}