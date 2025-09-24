using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Adınızı girin")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Email girin")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Şifre girin")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Şifreyi tekrar girin")]
    [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    [Required] public string Role { get; set; }
}