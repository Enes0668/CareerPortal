using KariyerPortali.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace KariyerPortali.Controllers
{

    public class JobController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // Herkese açık: iş ilanlarını listele
        public async Task<IActionResult> Index()
        {
            var jobs = await _db.Jobs.Include(j => j.PostedBy).OrderByDescending(j => j.PostedDate).ToListAsync();
            return View(jobs);
        }

        // Detay + başvuru butonu
        public async Task<IActionResult> Details(int id)
        {
            var job = await _db.Jobs.Include(j => j.PostedBy).Include(j => j.Applications).FirstOrDefaultAsync(j => j.Id == id);
            if (job == null) return NotFound();
            return View(job);
        }

        // Sadece işveren ekleyebilir
        [Authorize(Roles = "Employer")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "Employer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Job model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            model.PostedById = user.Id;
            model.PostedDate = DateTime.UtcNow;

            _db.Jobs.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Adaylar başvuru yapabilir
        [Authorize(Roles = "JobSeeker")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(int jobId, string coverLetter)
        {
            var user = await _userManager.GetUserAsync(User);
            var job = await _db.Jobs.FindAsync(jobId);
            if (job == null) return NotFound();

            // İki defa başvuru engeli
            var already = await _db.JobApplications.AnyAsync(a => a.JobId == jobId && a.ApplicantId == user.Id);
            if (already)
            {
                TempData["Message"] = "Bu ilana zaten başvurdunuz.";
                return RedirectToAction("Details", new { id = jobId });
            }

            var app = new JobApplication
            {
                JobId = jobId,
                ApplicantId = user.Id,
                CoverLetter = coverLetter
            };

            _db.JobApplications.Add(app);
            await _db.SaveChangesAsync();

            // istersen işverene email bildirimi gönder
            TempData["Message"] = "Başvurunuz alındı.";
            return RedirectToAction("Details", new { id = jobId });
        }

        // İşverenin kendi ilanları ve başvurular
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> MyJobs()
        {
            var user = await _userManager.GetUserAsync(User);
            var jobs = await _db.Jobs
                .Where(j => j.PostedById == user.Id)
                .Include(j => j.Applications).ThenInclude(a => a.Applicant)
                .ToListAsync();
            return View(jobs);
        }
    }
}
