using System;
using System.ComponentModel.DataAnnotations;

namespace KariyerPortalı.Models
{
    public class JobPosting
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Başlık")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Lokasyon")]
        public string Location { get; set; }

        [Display(Name = "Maaş Aralığı")]
        public string SalaryRange { get; set; }

        [Display(Name = "Yayın Tarihi")]
        public DateTime PostedDate { get; set; } = DateTime.Now;

        // İlanı hangi işveren verdi
        public string EmployerId { get; set; }
        public ApplicationUser Employer { get; set; }
    }
}
