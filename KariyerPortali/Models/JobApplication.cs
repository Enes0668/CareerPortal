using Microsoft.AspNetCore.Mvc;

namespace KariyerPortali.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }

        public string ApplicantId { get; set; }
        public ApplicationUser Applicant { get; set; }

        public string CoverLetter { get; set; }
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    }

}
