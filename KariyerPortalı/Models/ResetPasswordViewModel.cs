using System.ComponentModel.DataAnnotations;

public class ResetPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalı")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
    public string ConfirmPassword { get; set; }

    public string Token { get; set; } // Reset token
}
