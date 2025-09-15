using KariyerPortalı.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "JobSeeker")]
public class ApplicationController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _env;

    public ApplicationController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
    {
        _db = db;
        _userManager = userManager;
        _env = env;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Apply(int JobId, IFormFile CvFile)
    {
        if (CvFile == null || CvFile.Length == 0)
            return BadRequest("CV yüklenmedi.");

        var user = await _userManager.GetUserAsync(User);

        // Daha önce başvurmuş mu kontrol et
        var alreadyApplied = await _db.Applications
            .AnyAsync(a => a.JobId == JobId.ToString() && a.UserId == user.Id);

        if (alreadyApplied)
        {
            TempData["Error"] = "Bu ilana zaten başvurdunuz.";
            return RedirectToAction("Index", "Home"); // veya ilan detay sayfası
        }

        // CV kaydetme
        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads/cvs");
        Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + CvFile.FileName;
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await CvFile.CopyToAsync(stream);
        }

        // Başvuru kaydet
        var application = new Application
        {
            JobId = JobId.ToString(),
            UserId = user.Id,
            CvFilePath = "/uploads/cvs/" + uniqueFileName,
            AppliedDate = DateTime.Now
        };

        _db.Add(application);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Başvurunuz başarıyla gönderildi.";
        return RedirectToAction("Index", "Home");
    }
}
