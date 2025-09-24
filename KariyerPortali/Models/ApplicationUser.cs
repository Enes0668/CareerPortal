using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
    public string? ProfileImageUrl { get; set; }   // wwwroot/uploads/profiles/... veya dış storage url
    public string? Bio { get; set; }
    public string? Location { get; set; }
}