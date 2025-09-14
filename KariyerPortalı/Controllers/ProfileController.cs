using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KariyerPortalı.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace KariyerPortalı.Controllers
{
    // İstersen [Authorize] kaldırabilirsin test için
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public ProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        // Basit test endpoint
        [AllowAnonymous] // login olmadan da çalışır
        public IActionResult Test()
        {
            return Content("Profile controller çalışıyor!");
        }

        // GET: /Profile
        public IActionResult Index()
        {
            // User.Identity.Name ile kullanıcıyı bul
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Content("User bulunamadı");

            var user = _db.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
                return Content("DB’de user yok");

            var model = new ProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                Bio = user.Bio,
                Location = user.Location,
                ExistingImageUrl = user.ProfileImageUrl
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Content("User bulunamadı");

            var user = _db.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
                return Content("DB’de user yok");

            var model = new ProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                Bio = user.Bio,
                Location = user.Location,
                ExistingImageUrl = user.ProfileImageUrl
            };

            return View(model);
        }


        // POST: /Profile/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileViewModel model)
        {
            // Kullanıcıyı email üzerinden al
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Content("User bulunamadı, login misin?");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
                return Content("DB'de kullanıcı bulunamadı");

            // Model geçerliliğini kontrol et
            if (!ModelState.IsValid)
                return View(model);

            // Kullanıcı bilgilerini güncelle
            user.FullName = model.FullName;
            user.Bio = model.Bio;
            user.Location = model.Location;

            // Profil resmi yükleme
            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                // wwwroot/uploads klasörünü oluştur (varsa atla)
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Dosya adı ve yolu
                var fileName = Guid.NewGuid() + Path.GetExtension(model.ProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Dosyayı kaydet
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }

                // Kullanıcıya resmin URL’sini ata
                user.ProfileImageUrl = "/uploads/" + fileName;
            }

            // Veritabanında güncelle
            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        // Test için login olmadan edit endpoint
        [AllowAnonymous]
        [HttpGet("Profile/TestEdit")]
        public IActionResult TestEdit()
        {
            return Content("Edit endpoint test başarılı!");
        }

    }
}
