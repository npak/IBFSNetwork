using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBFSNetwork.Data;
using IBFSNetwork.Models;
using Microsoft.AspNetCore.Authorization;
using IBFSNetwork.Services;
using Microsoft.Extensions.Options;

namespace IBFSNetwork.Controllers
{
    
    public class SurveysController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly Codes.Settings _settings;
        public SurveysController(ApplicationDbContext context, IEmailSender emailSender, IOptions<Codes.Settings> settings)
        {
            _context = context;
            _emailSender = emailSender;
            _settings = settings.Value;
        }

        // GET: QuestionnaireModels
        public async Task<IActionResult> Index()
        {
            ViewBag.Questionnaire = _context.Questionnaires.Where(i => !i.Deleted);
            return View(await _context.QuestionnaireModels.Include(a => a.QuestionnaireAnswers).ToListAsync());
        }

        // GET: QuestionnaireModels/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(questionnaireModel);
        }

        // GET: QuestionnaireModels/Create
        public IActionResult Create()
        {
            ViewBag.Questionnaire = _context.Questionnaires.Where(q=> !q.Deleted).ToList();
            return View();
        }

        // POST: QuestionnaireModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateOn,Name,QuestionnaireAnswers")] QuestionnaireModel questionnaireModel)
        {
            if (ModelState.IsValid)
            {
                questionnaireModel.DateOn = DateTime.Now;
                _context.Add(questionnaireModel);
                await _context.SaveChangesAsync();
                //send to admin
                var message = "";
                int i = 1;
                foreach (var answ in questionnaireModel.QuestionnaireAnswers)
                {
                    message += i.ToString() + ". " + answ.AnswerText + Environment.NewLine;
                    i++;
                }
                await _emailSender.SendEmailAsync(_settings.SurveyEmail, "Survey answers. Date : "+ DateTime.Now,
                               message);

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

        // POST: QuestionnaireModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateOn,Name,QuestionnaireAnswers")] QuestionnaireModel questionnaireModel)
        {
            if (id != questionnaireModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questionnaireModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionnaireModelExists(questionnaireModel.Id))
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
            return View(questionnaireModel);
        }

        // GET: QuestionnaireModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(questionnaireModel);
        }

        // POST: QuestionnaireModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questionnaireModel = await _context.QuestionnaireModels.SingleOrDefaultAsync(m => m.Id == id);
            foreach(var ans in questionnaireModel.QuestionnaireAnswers)
            {
                var answer = _context.QuestionnaireAnswers.Where(a => a.AnswerId == ans.AnswerId).SingleOrDefault();
                if (answer != null)
                    _context.QuestionnaireAnswers.Remove(answer);
            }

            _context.QuestionnaireModels.Remove(questionnaireModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool QuestionnaireModelExists(int id)
        {
            return _context.QuestionnaireModels.Any(e => e.Id == id);
        }
    }
}
