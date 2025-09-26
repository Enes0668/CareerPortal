using Microsoft.AspNetCore.Builder;

namespace KariyerPortali.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public decimal? Salary { get; set; }
        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        public string PostedById { get; set; } // ApplicationUser Id
        public ApplicationUser PostedBy { get; set; }

        public ICollection<JobApplication> Applications { get; set; }
    }

}
