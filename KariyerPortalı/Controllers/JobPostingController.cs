using KariyerPortalı.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KariyerPortalı.Controllers
{
    [Authorize(Roles = "Employer")]
    public class JobPostingController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public JobPostingController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: Create Job Posting
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create Job Posting
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobPosting model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                model.EmployerId = user.Id;
                model.PostedDate = DateTime.Now;

                _db.JobPostings.Add(model);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [AllowAnonymous] // herkes görebilsin diye ekledim, sadece Employer değil
        public async Task<IActionResult> Details(int id)
        {
            var job = await _db.JobPostings.FindAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }
        // GET: Edit Job Posting
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound(); // id yoksa hata döndür

            var job = await _db.JobPostings.FindAsync(id);
            if (job == null)
                return NotFound(); // job yoksa hata döndür

            var user = await _userManager.GetUserAsync(User);
            if (job.EmployerId != user.Id)
                return Forbid(); // başkasının ilanını düzenlemeye çalışırsa engelle

            // Cache kontrolü (geri tuşta sorun yaşamamak için)
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return View(job);
        }

        // POST: Edit Job Posting
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JobPosting model)
        {
            if (id != model.Id)
                return BadRequest();

            var job = await _db.JobPostings.FindAsync(id);
            if (job == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (job.EmployerId != user.Id)
                return Forbid();

            if (ModelState.IsValid)
            {
                job.Title = model.Title;
                job.Description = model.Description;
                job.Location = model.Location;
                // diğer alanları ekle

                await _db.SaveChangesAsync();

                // Post-Redirect-Get (PRG) pattern: kaydettikten sonra GET ile yönlendir
                return RedirectToAction("MyJobPostings", "Profile");
            }

            // ModelState geçersiz ise aynı sayfaya geri dön
            return View(model);
        }


        // GET: Delete Job Posting
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var job = await _db.JobPostings.FindAsync(id);
            if (job == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (job.EmployerId != user.Id) return Forbid(); // Sadece sahibi silebilir

            _db.JobPostings.Remove(job);
            await _db.SaveChangesAsync();

            return RedirectToAction("MyJobPostings", "Profile");
        }

        // POST: Delete Job Posting
        
        [AllowAnonymous] // Herkes arama yapabilir
        public async Task<IActionResult> Index(string query)
        {
            // Tüm ilanlar
            var jobs = from j in _db.JobPostings
                       select j;

            // Arama varsa filtrele
            if (!string.IsNullOrEmpty(query))
            {
                jobs = jobs.Where(j => j.Title.Contains(query)
                                    || j.Description.Contains(query)
                                    || j.Location.Contains(query));
            }

            var jobList = await jobs.ToListAsync();
            return View(jobList); // Index.cshtml'i döndür
        }

        [HttpGet]
        public async Task<IActionResult> Applications(int jobId)
        {
            var applications = await _db.Applications
                .Where(a => a.JobId == jobId)
                .Include(a => a.User) // Kullanıcı bilgisi
                .ToListAsync();

            return PartialView("_ApplicationsPartial", applications);
        }


    }
}
