using KariyerPortali.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Job> Jobs { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<JobPosting> JobPostings { get; set; }
    public DbSet<Application> Applications { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // JobApplication -> Job
        modelBuilder.Entity<JobApplication>()
            .HasOne(j => j.Job)
            .WithMany(job => job.Applications)
            .HasForeignKey(j => j.JobId)
            .OnDelete(DeleteBehavior.Cascade); // Job silindiğinde uygulamalar silinsin

        // JobApplication -> Applicant
        modelBuilder.Entity<JobApplication>()
            .HasOne(j => j.Applicant)
            .WithMany(user => user.Applications)
            .HasForeignKey(j => j.ApplicantId)
            .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinse de uygulamalar silinmez
    }
}