using KariyerPortalı.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    }
}
