using Microsoft.AspNetCore.Mvc;
using System;

namespace KariyerPortalı.Controllers
{
    public class JobController : Controller
    {
        private readonly ApplicationDbContext _context;
        public JobController(ApplicationDbContext context) => _context = context;

        public IActionResult Index()
        {
            var jobs = _context.Jobs.ToList();
            return View(jobs);
        }

        public IActionResult Details(int id)
        {
            var job = _context.Jobs.FirstOrDefault(j => j.Id == id);
            if (job == null) return NotFound();
            return View(job);
        }
    }

}
