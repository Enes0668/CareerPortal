using Microsoft.AspNetCore.Http;

namespace KariyerPortalı.Models
{
    public class ProfileViewModel
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Bio { get; set; } = "";
        public string Location { get; set; } = "";
        public IFormFile? ProfileImage { get; set; }
        public string ExistingImageUrl { get; set; } = "";
    }
}
