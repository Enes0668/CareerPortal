using KariyerPortalı.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;

[Authorize(Roles = "Employer")]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        try
        {
            // Claim'den userId al
            var claimsIdentity = User.Identity as ClaimsIdentity;
            string userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            // Bu işverene ait ilanları al
            var jobPostings = _context.JobPostings
                                      .Where(j => j.EmployerId == userId)
                                      .ToList();

            var viewModel = new DashboardViewModel
            {
                JobTitles = jobPostings.Select(j => j.Title).ToList(),
                ApplicationsPerJob = jobPostings
                                     .Select(j => _context.Applications.Count(a => a.JobId == j.Id)) // Senin istediğin mantık
                                     .ToList()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return Content("Hata: " + ex.ToString());
        }
    }
    [HttpGet]
    public JsonResult GetChartData()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Json(new { labels = new string[] { }, values = new int[] { } });

        var jobPostings = _context.JobPostings
                                  .Where(j => j.EmployerId == userId)
                                  .ToList();

        var labels = jobPostings.Select(j => j.Title).ToList();
        var values = jobPostings.Select(j => _context.Applications.Count(a => a.JobId == j.Id)).ToList();

        return Json(new { labels, values });
    }
}
