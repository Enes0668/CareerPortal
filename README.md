# CareerPortal — Job Board & Recruitment Platform

A full-stack job board built with **ASP.NET Core 8 MVC**, where job seekers browse postings and apply with a CV upload, and employers manage their own listings and review incoming applications on a dashboard. The project uses **ASP.NET Core Identity** end to end: role-based authorization, mandatory email confirmation, and a token-based password reset flow.

## Features

**Authentication — ASP.NET Core Identity**
- Registration with role selection (Job Seeker / Employer)
- Mandatory email confirmation before login (confirmation link with Identity tokens, sent via SMTP)
- Forgot / reset password flow with secure reset tokens
- Roles (`JobSeeker`, `Employer`, `Admin`) and an admin account seeded automatically on startup

**Authorization & Security**
- Role-protected controllers (`[Authorize(Roles = "Employer")]` etc.)
- Ownership checks: an employer can only edit or delete their **own** postings (`Forbid()` otherwise)
- Anti-forgery tokens on all state-changing POST actions
- Uploaded CVs stored with GUID-prefixed filenames to prevent collisions and path guessing

**Job Postings**
- Employers create, edit and delete postings (title, description, location, salary range)
- Public listing page with keyword search across title, description and location
- Post-Redirect-Get pattern after form submissions

**Applications**
- Job seekers apply to a posting by uploading a CV (stored under `wwwroot/uploads/cvs`)
- Duplicate applications to the same posting are blocked
- Employers view applicants per posting via an AJAX-loaded partial view

**Employer Dashboard**
- Applications-per-posting statistics rendered with Chart.js
- Data served from a JSON endpoint (`GetChartData`) consumed by the chart on the client

**Profile Management**
- Custom `ApplicationUser` extending `IdentityUser` with full name, bio, location and profile image
- Profile edit page with image upload

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8.0 MVC |
| ORM | Entity Framework Core 8 — Code-First, Migrations, Fluent API relationships |
| Database | SQL Server |
| Auth | ASP.NET Core Identity (roles, email confirmation, password reset tokens) |
| Email | SMTP (`System.Net.Mail`), configured via `appsettings.json` |
| Frontend | Razor Views, Bootstrap 5, jQuery/AJAX, Chart.js |
| Deployment | Docker |

## Project Structure

```
KariyerPortali/
├── Controllers/     # Account, JobPosting, Application, Dashboard, Profile, Home
├── Models/          # Entities (JobPosting, Application) + ViewModels
├── Services/        # EmailSender (SMTP)
├── Data/            # ApplicationDbContext : IdentityDbContext<ApplicationUser>
├── Migrations/      # EF Core migrations
├── Views/           # Razor views (Account, JobPosting, Dashboard, Profile, ...)
├── wwwroot/         # Static assets, uploaded CVs and profile images
└── Dockerfile
```

Entity relationships are configured with the Fluent API in `OnModelCreating`, including delete behaviors (cascade for postings → applications, restrict for users → applications).

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB works for development)
- An SMTP account for confirmation emails (e.g. Gmail app password)

### Run locally

```bash
git clone https://github.com/Enes0668/CareerPortal.git
cd CareerPortal/KariyerPortali

# Configure appsettings.json:
#   ConnectionStrings:DefaultConnection -> your SQL Server connection string
#   EmailSettings -> SmtpServer, Port, SenderEmail, SenderName, Password

dotnet ef database update
dotnet run
```

On first run the app seeds the three roles and a default admin user. Change the admin credentials in `Program.cs` (or move them to configuration) before deploying anywhere public.

### Run with Docker

```bash
cd KariyerPortali
docker build -t careerportal .
docker run -p 8080:8080 -e ConnectionStrings__DefaultConnection="<your-connection-string>" careerportal
```

## Roadmap

- [ ] Filtering by salary range and date (search currently covers title/description/location)
- [ ] Email notification to the employer when a new application arrives
- [ ] Application status tracking (pending / reviewed / rejected)
- [ ] Structured logging with Serilog
- [ ] Unit and integration tests

## What I Learned

This project gave me hands-on experience with the full ASP.NET Core Identity pipeline — roles, email confirmation tokens and password reset tokens — plus resource-level authorization (ownership checks on top of role checks), secure file upload handling, and modeling a two-sided marketplace (seekers ↔ postings ↔ applications) with EF Core and the Fluent API.

---

*Built by [Enes Melih Eroğlu](https://github.com/Enes0668) — backend engineer focused on .NET.*
