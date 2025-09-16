using Microsoft.AspNetCore.Mvc;

namespace KariyerPortalı.Models
{
    public class Application
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string UserId { get; set; }
        public string CvFilePath { get; set; }
        public DateTime AppliedDate { get; set; }
    }
}
